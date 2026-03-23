namespace FestivalApi.Models;

public enum LineupDay
{
    LongTranh = 1,
    HoDau = 2
}

public class Band
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
    public string? HeroUrl { get; set; }
    public string? LogoUrl { get; set; }
    public LineupDay LineupDay { get; set; }
    public int LineupPosition { get; set; }
}
