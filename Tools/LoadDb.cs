using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PooLandApp.Data;
using PooLandApp.Server;

public static class LoadDb
{
    public static async Task LoadDbData(DbContextOptions<PooLandDbContext> options, MaxBound maxBound, DateTime fromDate, int numberOfPoo)
    {
        // empty to avoid logging while inserting (otherwise will flood console)
        var factory = new LoggerFactory();
        var builder = new DbContextOptionsBuilder<PooLandDbContext>(options)
          .UseLoggerFactory(factory);

        using var context = new PooLandDbContext(builder.Options);
        // result is true if the database had to be created

        await context.Database.EnsureDeletedAsync();
        if (await context.Database.EnsureCreatedAsync())
        {
            var seed = new SeedPoos(context, maxBound, fromDate, numberOfPoo);
            await seed.SeedDatabaseWithContactCountOfAsync();
        }
    }

    public static async Task<bool> EnsureCreated(DbContextOptions<PooLandDbContext> options) 
    {
        
        var builder = new DbContextOptionsBuilder<PooLandDbContext>(options);

        using var context = new PooLandDbContext(builder.Options);
        // result is true if the database had to be created

        return await context.Database.EnsureCreatedAsync();
    }


}

public class SeedPoos
{

    private PooLandDbContext DbContext;
    private MaxBound MaxBound;
    private DateTime FromDate;
    private int NumberOfPoo;
    public SeedPoos(PooLandDbContext dbContext, MaxBound maxBound, DateTime fromDate, int numberOfPoo)
    {
        DbContext = dbContext;
        MaxBound = maxBound;
        FromDate = fromDate;
        NumberOfPoo = numberOfPoo;
    }

    private Poodatum MakeNewPoo()
    {
        var date = GetRandom.GetDate(FromDate);
        var poo = new Poodatum
        {

            Date = date,
            Description = $"Poo genreted Auto {date}",
            Latitude = GetRandom.GetFloat(MaxBound.Up.Latitude, MaxBound.Down.Latitude),
            Longitude = GetRandom.GetFloat(MaxBound.Up.Longitude, MaxBound.Down.Longitude),
            Photo = "data:image/png;base64,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"

        };
        return poo;
    }
    public async Task SeedDatabaseWithContactCountOfAsync()
    {

        for (int i = 0; i < NumberOfPoo; i++)
        {
            DbContext.Add(MakeNewPoo());
        }
        await DbContext.SaveChangesAsync();

    }
}


public static class GetRandom
{
    public static DateTime GetDate(DateTime startDate)
    {
        var randomTest = new Random();

        TimeSpan timeSpan = DateTime.UtcNow - startDate;
        TimeSpan newSpan = new TimeSpan(0, randomTest.Next(0, (int)timeSpan.TotalMinutes), 0);
        DateTime newDate = startDate + newSpan;
        return newDate;
    }

    public static float GetFloat(float min, float max)
    {
        System.Random random = new System.Random();
        double val = (random.NextDouble() * (max - min) + min);
        return (float)val;
    }
}


