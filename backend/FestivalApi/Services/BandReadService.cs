using FestivalApi.Controllers;
using FestivalApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace FestivalApi.Services;

public class BandReadService
{
    private readonly FestivalDbContext _db;
    private readonly IMemoryCache _cache;

    private const string AllBandsCacheKey = "bands:all";
    private const string FeaturedBandsCacheKey = "bands:featured";
    private const string LineupBandsCacheKey = "bands:lineup";
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(2);

    public BandReadService(FestivalDbContext db, IMemoryCache cache)
    {
        _db = db;
        _cache = cache;
    }

    public async Task<List<BandsController.BandListDto>> GetBandsAsync(
        bool featured, CancellationToken ct = default)
    {
        var key = featured ? FeaturedBandsCacheKey : AllBandsCacheKey;
        if (_cache.TryGetValue(key, out List<BandsController.BandListDto>? cached) && cached != null)
            return cached;

        var query = _db.Bands.AsNoTracking().AsQueryable();
        if (featured)
        {
            query = query
                .Where(b => b.IsFeaturedOnHome)
                .OrderBy(b => b.LineupPosition)
                .Take(11);
        }
        else
        {
            query = query.OrderBy(b => b.LineupPosition);
        }

        var bands = await query
            .Select(b => new BandsController.BandListDto(
                b.Id,
                b.IsSecret ? "???" : b.Name,
                b.IsSecret ? null : b.HeroUrl,
                b.IsSecret ? null : b.LogoUrl,
                b.IsFeaturedOnHome,
                b.IsSecret,
                b.LineupDay,
                b.LineupPosition
            ))
            .ToListAsync(ct);

        _cache.Set(key, bands, CacheDuration);
        return bands;
    }

    public async Task<List<BandsController.LineupBandDto>> GetLineupBandsAsync(
        CancellationToken ct = default)
    {
        if (_cache.TryGetValue(LineupBandsCacheKey, out List<BandsController.LineupBandDto>? cached) && cached != null)
            return cached;

        var bands = await _db.Bands
            .AsNoTracking()
            .OrderBy(b => b.LineupPosition)
            .Select(b => new BandsController.LineupBandDto(
                b.Id,
                b.IsSecret ? "???" : b.Name,
                b.IsSecret ? null : b.LogoUrl,
                b.IsSecret,
                b.LineupDay,
                b.LineupPosition
            ))
            .ToListAsync(ct);

        _cache.Set(LineupBandsCacheKey, bands, CacheDuration);
        return bands;
    }
}
