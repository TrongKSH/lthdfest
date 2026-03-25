using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Caching.Memory;

namespace FestivalApi.Services;

/// <summary>
/// Tracks successful payment proof uploads keyed by resume token (hash only), so another client (e.g. desktop)
/// can poll for completion. In-memory only; not shared across scaled-out instances unless using a shared cache later.
/// </summary>
public sealed class ResumeProofCompletionTracker
{
    private const string KeyPrefix = "resume-proof-done:";
    private static readonly TimeSpan CompletionTtl = TimeSpan.FromHours(24);

    private readonly IMemoryCache _cache;

    public ResumeProofCompletionTracker(IMemoryCache cache)
    {
        _cache = cache;
    }

    public static string CacheKeyForToken(string resumeToken)
    {
        var bytes = Encoding.UTF8.GetBytes(resumeToken);
        var hash = SHA256.HashData(bytes);
        return KeyPrefix + Convert.ToHexString(hash).ToLowerInvariant();
    }

    public void MarkCompleted(string resumeToken)
    {
        if (string.IsNullOrWhiteSpace(resumeToken))
            return;

        var key = CacheKeyForToken(resumeToken);
        _cache.Set(key, true, CompletionTtl);
    }

    public bool IsCompleted(string resumeToken)
    {
        if (string.IsNullOrWhiteSpace(resumeToken))
            return false;

        var key = CacheKeyForToken(resumeToken);
        return _cache.TryGetValue(key, out bool value) && value;
    }
}
