namespace FestivalApi.Models;

public class CountdownDto
{
    public string EventDate { get; set; } = string.Empty; // ISO8601
    public string EventName { get; set; } = string.Empty;
}
