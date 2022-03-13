using PooLandApp.Data;
using PooLandApp.Server;
using AspNetMonsters.Blazor.Geolocation;
using Radzen;
using Texnomic.Blazor.hCaptcha.Extensions;
using BlazorStrap;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

//builder.Services.AddServerSideBlazor().AddCircuitOptions(e => {
//    e.DetailedErrors = true;
//});


#if DEBUG
builder.Services.AddDbContextFactory<PooLandDbContext>(options =>
   options.UseSqlite($"DataSource={builder.Configuration["DbName"]};", x => x.UseNetTopologySuite())
     .EnableSensitiveDataLogging());
#else
builder.Services.AddDbContextFactory<PooLandDbContext>(options =>
   options.UseSqlite($"DataSource={builder.Configuration["DbName"]};", x => x.UseNetTopologySuite()));
#endif


builder.Services.Configure<LeafletOptions>(builder.Configuration.GetSection("Leaflet"));
builder.Services.Configure<DataOptions>(builder.Configuration.GetSection("DataOptions"));
builder.Services.Configure<AdminBoardOptions>(builder.Configuration.GetSection("AdminBoard"));
builder.Services.Configure<PhotoOptions>(builder.Configuration.GetSection("PhotoOptions"));

builder.Services.AddScoped<LocationService>();
builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();

builder.Services.Configure<hCaptchaOptions>(builder.Configuration.GetSection("hCaptcha"));
builder.Services.AddHttpClient();
builder.Services.AddHCaptcha(Options =>
{
    Options.SiteKey = builder.Configuration["hCaptcha:SiteKey"];
    Options.Secret = builder.Configuration["hCaptcha:Secret"];
});

builder.Services.AddBlazorStrap();

Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .CreateLogger(); 
builder.Services.AddLogging(logging => {
    logging.ClearProviders();
    logging.AddSerilog();
});

builder.Services.AddLocalization();
builder.Services.AddControllers();

var app = builder.Build();


await using var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateAsyncScope();
var options = scope.ServiceProvider.GetRequiredService<DbContextOptions<PooLandDbContext>>();
#if DEBUG
    // this section sets up and seeds the database. 
    Log.Information("Debug Mode, creating db");
    var maxBound = scope.ServiceProvider.GetService<IOptionsMonitor<LeafletOptions>>().CurrentValue.MaxBounds;
    await LoadDb.LoadDbData(options, maxBound, DateTime.UtcNow.AddYears(-1), 1000);
    Log.Information("Debug Mode, created db");
#else
//Ensure DB has the last model
    if (await LoadDb.EnsureCreated(builder.Configuration["DbName"],options))
        Log.Information("Database created");
    else
        Log.Information("Database already exists");
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

var supportedCultures = new[] { "en", "es" };
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[1])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);

app.UseRequestLocalization(localizationOptions);

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
