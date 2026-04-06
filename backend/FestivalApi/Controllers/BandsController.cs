using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FestivalApi.Data;
using FestivalApi.Services;

namespace FestivalApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BandsController : ControllerBase
{
    private readonly FestivalDbContext _db;
    private readonly BandReadService _bandReadService;

    public sealed record BandListDto(
        int Id,
        string Name,
        string? HeroUrl,
        string? LogoUrl,
        bool IsFeaturedOnHome,
        bool IsSecret,
        Models.LineupDay LineupDay,
        int LineupPosition
    );

    public sealed record LineupBandDto(
        int Id,
        string Name,
        string? LogoUrl,
        bool IsSecret,
        Models.LineupDay LineupDay,
        int LineupPosition
    );

    public BandsController(FestivalDbContext db, BandReadService bandReadService)
    {
        _db = db;
        _bandReadService = bandReadService;
    }

    /// <summary>
    /// List all bands ordered by lineup position. Optional ?featured=true filter.
    /// Returns a slim DTO without Bio/BioEn to reduce payload size.
    /// </summary>
    [HttpGet]
    [ResponseCache(Duration = 120, Location = ResponseCacheLocation.Any)]
    public async Task<ActionResult<IEnumerable<BandListDto>>> GetBands(
        [FromQuery] bool featured = false,
        CancellationToken cancellationToken = default)
    {
        var bands = await _bandReadService.GetBandsAsync(featured, cancellationToken);
        return Ok(bands);
    }

    /// <summary>
    /// Lightweight lineup payload for logo grid rendering.
    /// </summary>
    [HttpGet("lineup")]
    [ResponseCache(Duration = 120, Location = ResponseCacheLocation.Any)]
    public async Task<ActionResult<IEnumerable<LineupBandDto>>> GetLineupBands(
        CancellationToken cancellationToken = default)
    {
        var bands = await _bandReadService.GetLineupBandsAsync(cancellationToken);
        return Ok(bands);
    }

    /// <summary>
    /// Get a single band by id for detail page or modal.
    /// </summary>
    [HttpGet("{id:int}")]
    [ResponseCache(Duration = 120, Location = ResponseCacheLocation.Any)]
    public async Task<ActionResult<Models.Band>> GetBand(int id, CancellationToken cancellationToken = default)
    {
        var band = await _db.Bands
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.Id == id && !b.IsSecret, cancellationToken);
        if (band == null)
            return NotFound();
        return Ok(band);
    }

}
