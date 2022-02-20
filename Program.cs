using AspNetMonsters.Blazor.Geolocation;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PooLandApp.Data;
using PooLandApp.Server;
using Radzen;
using Texnomic.Blazor.hCaptcha.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();


#if DEBUG
builder.Services.AddDbContext<PooLandDbContext>(options =>
   options.UseSqlServer(builder.Configuration["ConnectionStrings:PooLandDb"])
     .EnableSensitiveDataLogging());
#else
builder.Services.AddDbContext<PooLandDbContext>(options =>
   options.UseSqlServer(builder.Configuration["ConnectionStrings:PooLandDb"])
    );
#endif


builder.Services.Configure<LeafletOptions>(builder.Configuration.GetSection("Leaflet"));
builder.Services.Configure<DataOptions>(builder.Configuration.GetSection("DataOptions"));

builder.Services.AddScoped<LocationService>();
builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();

builder.Services.AddHttpClient();
builder.Services.AddHCaptcha(Options =>
{
    Options.SiteKey = builder.Configuration["hCaptcha:SiteKey"];
    Options.Secret = builder.Configuration["hCaptcha:Secret"];
});

var app = builder.Build();

#if DEBUG
// this section sets up and seeds the database. It would NOT normally
// be done this way in production. It is here to make the sample easier,
// i.e. clone, set connection string and run.
await using var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateAsyncScope();
var options = scope.ServiceProvider.GetRequiredService<DbContextOptions<PooLandDbContext>>();
var maxBound = scope.ServiceProvider.GetService<IOptionsMonitor<LeafletOptions>>().CurrentValue.MaxBounds;
await LoadDb.LoadDbData(options, maxBound, DateTime.UtcNow.AddYears(-1), 1000);
#endif

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
