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
        Models.LineupDay LineupDay,
        int LineupPosition
    );

    public BandsController(FestivalDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// List all bands ordered by lineup position. Optional ?featured=true filter (for MVP returns all).
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Models.Band>>> GetBands(
        [FromQuery] bool featured = false,
        CancellationToken cancellationToken = default)
    {
        var query = _db.Bands.AsNoTracking().OrderBy(b => b.LineupPosition).AsQueryable();
        if (featured)
        {
            // For MVP: treat first N as featured, or add a Featured flag to Band later
            query = query.Take(6);
        }
        var bands = await query.ToListAsync(cancellationToken);
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
                b.Name,
                b.LogoUrl,
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
            .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
        if (band == null)
            return NotFound();
        return Ok(band);
    }
}
