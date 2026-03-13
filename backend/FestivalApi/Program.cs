using Microsoft.EntityFrameworkCore;
using FestivalApi.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<FestivalDbContext>(options =>
    options.UseInMemoryDatabase("FestivalDb"));

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<FestivalDbContext>();
    db.Database.EnsureCreated();
    // InMemory does not apply HasData from migrations; seed explicitly
    if (!db.Festivals.Any())
    {
        db.Festivals.Add(new FestivalApi.Models.Festival
        {
            Id = 1,
            Name = "Lễ hội Thăng Long",
            EventDate = new DateTime(2026, 5, 8, 18, 0, 0, DateTimeKind.Utc),
            Venue = "Hà Nội",
            Description = "Lễ hội âm nhạc quy mô lớn với hơn 20 ban nhạc.",
            ImageUrl = null
        });
        db.Bands.AddRange(
            new FestivalApi.Models.Band { Id = 1, Name = "Band Alpha", ShortBio = "Rock band from the capital.", ImageUrl = null, Genre = "Rock", LineupPosition = 1, SocialLinks = null },
            new FestivalApi.Models.Band { Id = 2, Name = "Band Beta", ShortBio = "Electronic and indie fusion.", ImageUrl = null, Genre = "Electronic", LineupPosition = 2, SocialLinks = null },
            new FestivalApi.Models.Band { Id = 3, Name = "Band Gamma", ShortBio = "Vietnamese traditional meets modern.", ImageUrl = null, Genre = "Fusion", LineupPosition = 3, SocialLinks = null },
            new FestivalApi.Models.Band { Id = 4, Name = "Band Delta", ShortBio = "Pop and R&B vibes.", ImageUrl = null, Genre = "Pop", LineupPosition = 4, SocialLinks = null },
            new FestivalApi.Models.Band { Id = 5, Name = "Band Epsilon", ShortBio = "Metal and hardcore.", ImageUrl = null, Genre = "Metal", LineupPosition = 5, SocialLinks = null }
        );
        db.SaveChanges();
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.MapControllers();

app.Run();
