using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FestivalApi.Data;
using FestivalApi.Models;

namespace FestivalApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CountdownController : ControllerBase
{
    private readonly FestivalDbContext _db;

    public CountdownController(FestivalDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Returns event date and name for frontend countdown (single source of truth).
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<CountdownDto>> Get(CancellationToken cancellationToken = default)
    {
        var festival = await _db.Festivals.OrderBy(f => f.EventDate).FirstOrDefaultAsync(cancellationToken);
        if (festival == null)
            return NotFound();
        return Ok(new CountdownDto
        {
            EventDate = festival.EventDate.ToString("O"),
            EventName = festival.Name
        });
    }
}
