using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Berrilan.Claims.Core.Persistence;

public class AppDbContextFactory()  : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[]? args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
        .AddUserSecrets<AppDbContextFactory>()
        .Build();

        DbContextOptionsBuilder<AppDbContext> optionsBuilder = new();
        optionsBuilder.UseNpgsql(configuration.GetConnectionString("AppConnection")).UseSnakeCaseNamingConvention();

        return new AppDbContext(optionsBuilder.Options);
    }
}
