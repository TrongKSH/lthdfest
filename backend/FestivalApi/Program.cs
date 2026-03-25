using Microsoft.EntityFrameworkCore;
using FestivalApi.Data;
using FestivalApi.Options;
using FestivalApi.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
builder.Services.Configure<GooglePaymentOptions>(
    builder.Configuration.GetSection(GooglePaymentOptions.SectionName));
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<GoogleDriveSheetsPaymentService>();
builder.Services.AddSingleton<TicketPaymentProofResumeTokenService>();
builder.Services.AddSingleton<ResumeProofCompletionTracker>();

builder.Services.Configure<Microsoft.AspNetCore.Http.Features.FormOptions>(o =>
{
    o.MultipartBodyLengthLimit = 10_485_760;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<FestivalDbContext>(options =>
    options.UseInMemoryDatabase("FestivalDb"));

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        var origins = new List<string>();

        // appsettings + Cors__AllowedOrigins__0, __1, ... on Render
        var fromConfig =
            builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();
        foreach (var o in fromConfig.Where(x => !string.IsNullOrWhiteSpace(x)))
            origins.Add(o.Trim());

        // One env var on Render: CORS_ALLOWED_ORIGINS=https://a.com,https://b.com
        var csv = builder.Configuration["CORS_ALLOWED_ORIGINS"];
        if (!string.IsNullOrWhiteSpace(csv))
        {
            foreach (var part in csv.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                origins.Add(part);
        }

        if (builder.Environment.IsDevelopment())
            origins.Add("http://localhost:4200");

        origins = origins.Distinct(StringComparer.OrdinalIgnoreCase).ToList();

        if (origins.Count > 0)
            policy.WithOrigins(origins.ToArray());
        else
            policy.WithOrigins("http://localhost:4200");

        policy.AllowAnyHeader().AllowAnyMethod();
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<FestivalDbContext>();
    db.Database.EnsureCreated();
    // InMemory does not apply HasData from migrations; seed explicitly.
    // Keep festivals and bands checks independent so bands are re-seeded
    // even if a festival row already exists.
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
    }

    if (!db.Bands.Any())
    {
        db.Bands.AddRange(BandSeedData.All);
    }

    db.SaveChanges();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.MapGet("/healthz", () => Results.Ok(new { status = "ok" }));
app.MapControllers();

app.Run();
