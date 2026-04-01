using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FestivalApi.Data;

namespace FestivalApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BandsController : ControllerBase
{
    private readonly FestivalDbContext _db;
    public sealed record LineupBandDto(
        int Id,
        string Name,
        string? LogoUrl,
        bool IsSecret,
        Models.LineupDay LineupDay,
        int LineupPosition
    );

    public BandsController(FestivalDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// List all bands ordered by lineup position. Optional ?featured=true filter.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Models.Band>>> GetBands(
        [FromQuery] bool featured = false,
        CancellationToken cancellationToken = default)
    {
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

        var bands = await query.ToListAsync(cancellationToken);
        AnonymizeSecretBands(bands);
        return Ok(bands);
    }

    /// <summary>
    /// Lightweight lineup payload for logo grid rendering.
    /// </summary>
    [HttpGet("lineup")]
    public async Task<ActionResult<IEnumerable<LineupBandDto>>> GetLineupBands(
        CancellationToken cancellationToken = default)
    {
        var bands = await _db.Bands
            .AsNoTracking()
            .OrderBy(b => b.LineupPosition)
            .Select(b => new LineupBandDto(
                b.Id,
                b.IsSecret ? "???" : b.Name,
                b.IsSecret ? null : b.LogoUrl,
                b.IsSecret,
                b.LineupDay,
                b.LineupPosition
            ))
            .ToListAsync(cancellationToken);

        return Ok(bands);
    }

    /// <summary>
    /// Get a single band by id for detail page or modal.
    /// </summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Models.Band>> GetBand(int id, CancellationToken cancellationToken = default)
    {
        var band = await _db.Bands
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.Id == id && !b.IsSecret, cancellationToken);
        if (band == null)
            return NotFound();
        return Ok(band);
    }

    private static void AnonymizeSecretBands(List<Models.Band> bands)
    {
        foreach (var b in bands)
        {
            if (!b.IsSecret) continue;
            b.Name = "???";
            b.Bio = string.Empty;
            b.BioEn = string.Empty;
            b.HeroUrl = null;
            b.LogoUrl = null;
        }
    }
}
