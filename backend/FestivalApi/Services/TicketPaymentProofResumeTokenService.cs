using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using FestivalApi.Options;
using Microsoft.Extensions.Options;

namespace FestivalApi.Services;

public sealed class TicketPaymentProofResumeTokenService
{
    private const int NonceSize = 12;
    private const int TagSize = 16;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    private readonly GooglePaymentOptions _options;
    private readonly byte[] _encryptionKey;

    public TicketPaymentProofResumeTokenService(IOptions<GooglePaymentOptions> options)
    {
        _options = options.Value;
        _encryptionKey = DeriveEncryptionKey(_options.ResumeTokenSecret);
    }

    public bool IsConfigured => _options.HasResumeTokenSecret;

    public DateTimeOffset GetExpiresAtUtc(DateTimeOffset nowUtc)
    {
        var ttlMinutes = Math.Clamp(_options.ResumeTokenTtlMinutes, 5, 240);
        return nowUtc.AddMinutes(ttlMinutes);
    }

    /// <summary>
    /// Creates an encrypted, authenticated token containing the PII payload.
    /// Format: Base64Url( nonce[12] || ciphertext[N] || tag[16] )
    /// </summary>
    public string CreateToken(
        string fullName,
        string phone,
        string email,
        string purchaseType,
        int qty,
        string merchSize,
        DateTimeOffset expiresAtUtc)
    {
        var payload = new ResumeTokenPayload(
            fullName.Trim(),
            phone.Trim(),
            email.Trim(),
            purchaseType.Trim(),
            qty,
            merchSize.Trim(),
            expiresAtUtc.ToUnixTimeSeconds());

        var payloadJson = JsonSerializer.SerializeToUtf8Bytes(payload, JsonOptions);

        var nonce = new byte[NonceSize];
        RandomNumberGenerator.Fill(nonce);

        var ciphertext = new byte[payloadJson.Length];
        var tag = new byte[TagSize];

        using var aes = new AesGcm(_encryptionKey, TagSize);
        aes.Encrypt(nonce, payloadJson, ciphertext, tag);

        var combined = new byte[NonceSize + ciphertext.Length + TagSize];
        nonce.CopyTo(combined, 0);
        ciphertext.CopyTo(combined, NonceSize);
        tag.CopyTo(combined, NonceSize + ciphertext.Length);

        return Base64UrlEncode(combined);
    }

    public bool TryReadToken(
        string token,
        DateTimeOffset nowUtc,
        out ResumeTokenPayload payload)
    {
        payload = default!;
        if (string.IsNullOrWhiteSpace(token) || !IsConfigured)
            return false;

        byte[] combined;
        try
        {
            combined = Base64UrlDecode(token);
        }
        catch
        {
            return false;
        }

        if (combined.Length < NonceSize + TagSize + 1)
            return false;

        var nonce = combined[..NonceSize];
        var ciphertext = combined[NonceSize..^TagSize];
        var tag = combined[^TagSize..];

        byte[] plaintext;
        try
        {
            plaintext = new byte[ciphertext.Length];
            using var aes = new AesGcm(_encryptionKey, TagSize);
            aes.Decrypt(nonce, ciphertext, tag, plaintext);
        }
        catch (CryptographicException)
        {
            return false;
        }

        ResumeTokenPayload? parsedPayload;
        try
        {
            parsedPayload = JsonSerializer.Deserialize<ResumeTokenPayload>(plaintext, JsonOptions);
        }
        catch
        {
            return false;
        }

        if (parsedPayload == null)
            return false;

        if (parsedPayload.Exp < nowUtc.ToUnixTimeSeconds())
            return false;

        if (string.IsNullOrWhiteSpace(parsedPayload.FullName)
            || string.IsNullOrWhiteSpace(parsedPayload.Phone)
            || string.IsNullOrWhiteSpace(parsedPayload.Email)
            || string.IsNullOrWhiteSpace(parsedPayload.PurchaseType)
            || parsedPayload.Qty < 1)
        {
            return false;
        }

        payload = parsedPayload;
        return true;
    }

    private static byte[] DeriveEncryptionKey(string? secret)
    {
        var keyMaterial = Encoding.UTF8.GetBytes(secret ?? string.Empty);
        return SHA256.HashData(keyMaterial);
    }

    private static string Base64UrlEncode(byte[] bytes)
    {
        return Convert.ToBase64String(bytes)
            .Replace('+', '-')
            .Replace('/', '_')
            .TrimEnd('=');
    }

    private static byte[] Base64UrlDecode(string value)
    {
        var padded = value.Replace('-', '+').Replace('_', '/');
        var remainder = padded.Length % 4;
        if (remainder > 0)
            padded = padded.PadRight(padded.Length + (4 - remainder), '=');
        return Convert.FromBase64String(padded);
    }
}

public sealed record ResumeTokenPayload(
    string FullName,
    string Phone,
    string Email,
    string PurchaseType,
    int Qty,
    string MerchSize,
    long Exp);
