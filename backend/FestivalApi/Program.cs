using Microsoft.EntityFrameworkCore;
using FestivalApi.Data;
using FestivalApi.Options;
using FestivalApi.Services;
using System.IO.Compression;
using System.Text.Json.Serialization;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.ResponseCompression;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });
builder.Services.Configure<GooglePaymentOptions>(
    builder.Configuration.GetSection(GooglePaymentOptions.SectionName));
builder.Services.AddMemoryCache();

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 60,
                Window = TimeSpan.FromMinutes(1),
                QueueLimit = 0,
            }));

    options.AddPolicy("payment", httpContext =>
        RateLimitPartition.GetSlidingWindowLimiter(
            httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            _ => new SlidingWindowRateLimiterOptions
            {
                PermitLimit = 10,
                Window = TimeSpan.FromMinutes(1),
                SegmentsPerWindow = 4,
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = 2,
            }));

    options.AddPolicy("polling", httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 120,
                Window = TimeSpan.FromMinutes(1),
                QueueLimit = 0,
            }));
});
builder.Services.AddScoped<FestivalReadService>();
builder.Services.AddScoped<BandReadService>();
builder.Services.AddSingleton<GoogleDriveSheetsPaymentService>();
builder.Services.AddSingleton<TicketPaymentProofResumeTokenService>();
builder.Services.AddSingleton<ResumeProofCompletionTracker>();

builder.Services.Configure<Microsoft.AspNetCore.Http.Features.FormOptions>(o =>
{
    o.MultipartBodyLengthLimit = 10_485_760;
});
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<BrotliCompressionProvider>();
    options.Providers.Add<GzipCompressionProvider>();
});
builder.Services.Configure<BrotliCompressionProviderOptions>(o => o.Level = CompressionLevel.Fastest);
builder.Services.Configure<GzipCompressionProviderOptions>(o => o.Level = CompressionLevel.Fastest);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<FestivalDbContext>(options =>
    options.UseInMemoryDatabase("FestivalDb"));

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        var origins = new List<string>();

        // appsettings + Cors__AllowedOrigins__0, __1, ... env vars
        var fromConfig =
            builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();
        foreach (var o in fromConfig.Where(x => !string.IsNullOrWhiteSpace(x)))
            origins.Add(o.Trim());

        // CORS_ALLOWED_ORIGINS=https://a.com,https://b.com
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

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<FestivalDbContext>();
    await db.Database.EnsureCreatedAsync();
    // InMemory does not apply HasData from migrations; seed explicitly.
    // Keep festivals and bands checks independent so bands are re-seeded
    // even if a festival row already exists.
    if (!await db.Festivals.AnyAsync())
    {
        db.Festivals.Add(new FestivalApi.Models.Festival
        {
            Id = 1,
            Name = "Lễ hội Thăng Long",
            EventDate = new DateTime(2026, 5, 8, 18, 0, 0, DateTimeKind.Utc),
            Venue = "Hà Nội",
            Description = "Lễ hội âm nhạc quy mô lớn với hơn 20 ban nhạc.",
            DescriptionEn = "Large-scale music festival with over 20 bands.",
            ImageUrl = null
        });
    }

    if (!await db.Bands.AnyAsync())
    {
        db.Bands.AddRange(BandSeedData.All);
    }

    await db.SaveChangesAsync();
}

app.UseForwardedHeaders();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseResponseCompression();
app.UseCors();
app.UseRateLimiter();
app.MapGet("/healthz", () => Results.Ok(new { status = "ok" }));
app.MapControllers();

app.Run();
