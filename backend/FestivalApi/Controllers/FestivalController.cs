using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FestivalApi.Data;
using FestivalApi.Models;

namespace FestivalApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FestivalController : ControllerBase
{
    private readonly FestivalDbContext _db;

    public FestivalController(FestivalDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Get the single festival (or first event) for hero/countdown and event info.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<Festival>> GetFestival(CancellationToken cancellationToken = default)
    {
        var festival = await _db.Festivals.OrderBy(f => f.EventDate).FirstOrDefaultAsync(cancellationToken);
        if (festival == null)
            return NotFound();
        return Ok(festival);
    }

}
