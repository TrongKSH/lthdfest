using Microsoft.AspNetCore.Mvc;
using FestivalApi.Models;
using FestivalApi.Services;

namespace FestivalApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FestivalController : ControllerBase
{
    private readonly FestivalReadService _festivalRead;

    public FestivalController(FestivalReadService festivalRead)
    {
        _festivalRead = festivalRead;
    }

    /// <summary>
    /// Get the single festival (or first event) for hero/countdown and event info.
    /// </summary>
    [HttpGet]
    [ResponseCache(Duration = 120, Location = ResponseCacheLocation.Any)]
    public async Task<ActionResult<Festival>> GetFestival(CancellationToken cancellationToken = default)
    {
        var festival = await _festivalRead.GetPrimaryFestivalAsync(cancellationToken);
        if (festival == null)
            return NotFound();
        return Ok(festival);
    }

}
