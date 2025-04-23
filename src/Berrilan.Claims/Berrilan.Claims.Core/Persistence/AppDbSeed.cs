using Berrilan.Claims.Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace Berrilan.Claims.Core.Persistence;

public static class AppDbSeed
{
    public static void Seed(this ModelBuilder modelBuilder)
    {
        User cliebana = new()
        {
            Id = Guid.Parse("0194efaa-0095-d8fd-7903-976365a009fd"),
            Email = "cliebana@berrilan.com",
            CustomerId = Guid.Parse("0194efa9-938a-7227-3ece-1c954289b196"),
            Role = Role.Admin,
            License = License.Enterprise,
            IsRoot = true
        };

        modelBuilder.Entity<User>().HasData(cliebana);
    }
}   

