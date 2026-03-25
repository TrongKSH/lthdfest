using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using FestivalApi.Options;
using Microsoft.Extensions.Options;

namespace FestivalApi.Services;

public sealed class TicketPaymentProofResumeTokenService
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    private readonly GooglePaymentOptions _options;

    public TicketPaymentProofResumeTokenService(IOptions<GooglePaymentOptions> options)
    {
        _options = options.Value;
    }

    public bool IsConfigured => _options.HasResumeTokenSecret;

    public DateTimeOffset GetExpiresAtUtc(DateTimeOffset nowUtc)
    {
        var ttlMinutes = Math.Clamp(_options.ResumeTokenTtlMinutes, 5, 240);
        return nowUtc.AddMinutes(ttlMinutes);
    }

    public string CreateToken(
        string fullName,
        string phone,
        string email,
        string purchaseType,
        int qty,
        DateTimeOffset expiresAtUtc)
    {
        var payload = new ResumeTokenPayload(
            fullName.Trim(),
            phone.Trim(),
            email.Trim(),
            purchaseType.Trim(),
            qty,
            expiresAtUtc.ToUnixTimeSeconds());

        var payloadJson = JsonSerializer.Serialize(payload, JsonOptions);
        var payloadPart = Base64UrlEncode(Encoding.UTF8.GetBytes(payloadJson));
        var signature = ComputeSignature(payloadPart);
        var signaturePart = Base64UrlEncode(signature);
        return $"{payloadPart}.{signaturePart}";
    }

    public bool TryReadToken(
        string token,
        DateTimeOffset nowUtc,
        out ResumeTokenPayload payload)
    {
        payload = default!;
        if (string.IsNullOrWhiteSpace(token) || !IsConfigured)
            return false;

        var separatorIndex = token.IndexOf('.', StringComparison.Ordinal);
        if (separatorIndex <= 0 || separatorIndex >= token.Length - 1)
            return false;

        var payloadPart = token[..separatorIndex];
        var signaturePart = token[(separatorIndex + 1)..];

        var expectedSignature = ComputeSignature(payloadPart);
        byte[] actualSignature;
        try
        {
            actualSignature = Base64UrlDecode(signaturePart);
        }
        catch
        {
            return false;
        }

        if (!CryptographicOperations.FixedTimeEquals(expectedSignature, actualSignature))
            return false;

        ResumeTokenPayload? parsedPayload;
        try
        {
            var payloadBytes = Base64UrlDecode(payloadPart);
            parsedPayload = JsonSerializer.Deserialize<ResumeTokenPayload>(payloadBytes, JsonOptions);
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

    private byte[] ComputeSignature(string payloadPart)
    {
        var secret = _options.ResumeTokenSecret ?? string.Empty;
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret));
        return hmac.ComputeHash(Encoding.UTF8.GetBytes(payloadPart));
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
    long Exp);
