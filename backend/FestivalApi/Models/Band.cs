namespace FestivalApi.Models;

public class Band
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ShortBio { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public string? Genre { get; set; }
    public int LineupPosition { get; set; }
    public string? SocialLinks { get; set; }
}
