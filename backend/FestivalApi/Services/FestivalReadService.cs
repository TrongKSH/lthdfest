using FestivalApi.Data;
using FestivalApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace FestivalApi.Services;

/// <summary>
/// Single read path for the primary festival row with short in-memory caching
/// (shared by festival and countdown endpoints).
/// </summary>
public class FestivalReadService
{
    private readonly FestivalDbContext _db;
    private readonly IMemoryCache _cache;

    private const string PrimaryFestivalCacheKey = "festival:primary";
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(1);

    public FestivalReadService(FestivalDbContext db, IMemoryCache cache)
    {
        _db = db;
        _cache = cache;
    }

    public async Task<Festival?> GetPrimaryFestivalAsync(CancellationToken cancellationToken = default)
    {
        if (_cache.TryGetValue(PrimaryFestivalCacheKey, out Festival? cached))
            return cached;

        var festival = await _db.Festivals
            .AsNoTracking()
            .OrderBy(f => f.EventDate)
            .FirstOrDefaultAsync(cancellationToken);

        if (festival != null)
            _cache.Set(PrimaryFestivalCacheKey, festival, CacheDuration);

        return festival;
    }
}
