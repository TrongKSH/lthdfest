using Microsoft.AspNetCore.Mvc;
using FestivalApi.Models;
using FestivalApi.Services;

namespace FestivalApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CountdownController : ControllerBase
{
    private readonly FestivalReadService _festivalRead;

    public CountdownController(FestivalReadService festivalRead)
    {
        _festivalRead = festivalRead;
    }

    /// <summary>
    /// Returns event date and name for frontend countdown (single source of truth).
    /// </summary>
    [HttpGet]
    [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
    public async Task<ActionResult<CountdownDto>> Get(CancellationToken cancellationToken = default)
    {
        var festival = await _festivalRead.GetPrimaryFestivalAsync(cancellationToken);
        if (festival == null)
            return NotFound();
        return Ok(new CountdownDto
        {
            EventDate = festival.EventDate.ToString("O"),
            EventName = festival.Name
        });
    }
}
