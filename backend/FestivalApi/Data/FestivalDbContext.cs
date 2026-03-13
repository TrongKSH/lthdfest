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
                ImageUrl = null
            }
        );

        // Seed sample bands (ordered by lineup position)
        modelBuilder.Entity<Band>().HasData(
            new Band { Id = 1, Name = "Band Alpha", ShortBio = "Rock band from the capital.", ImageUrl = null, Genre = "Rock", LineupPosition = 1, SocialLinks = null },
            new Band { Id = 2, Name = "Band Beta", ShortBio = "Electronic and indie fusion.", ImageUrl = null, Genre = "Electronic", LineupPosition = 2, SocialLinks = null },
            new Band { Id = 3, Name = "Band Gamma", ShortBio = "Vietnamese traditional meets modern.", ImageUrl = null, Genre = "Fusion", LineupPosition = 3, SocialLinks = null },
            new Band { Id = 4, Name = "Band Delta", ShortBio = "Pop and R&B vibes.", ImageUrl = null, Genre = "Pop", LineupPosition = 4, SocialLinks = null },
            new Band { Id = 5, Name = "Band Epsilon", ShortBio = "Metal and hardcore.", ImageUrl = null, Genre = "Metal", LineupPosition = 5, SocialLinks = null }
        );
    }
}
