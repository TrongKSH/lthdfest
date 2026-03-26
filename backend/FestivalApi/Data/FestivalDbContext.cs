using Microsoft.EntityFrameworkCore;
using FestivalApi.Models;

namespace FestivalApi.Data;

public class FestivalDbContext : DbContext
{
    public FestivalDbContext(DbContextOptions<FestivalDbContext> options)
        : base(options)
    {
    }

    public DbSet<Festival> Festivals => Set<Festival>();
    public DbSet<Band> Bands => Set<Band>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Seed one festival (8-9 May 2026)
        modelBuilder.Entity<Festival>().HasData(
            new Festival
            {
                Id = 1,
                Name = "Lễ hội Thăng Long",
                EventDate = new DateTime(2026, 5, 8, 18, 0, 0, DateTimeKind.Utc),
                Venue = "Hà Nội",
                Description = "Lễ hội âm nhạc quy mô lớn với hơn 20 ban nhạc.",
                DescriptionEn = "Large-scale music festival with over 20 bands.",
                ImageUrl = null
            }
        );

        // Bands are seeded from BandSeedData in Program.cs (single place to update).
    }
}
