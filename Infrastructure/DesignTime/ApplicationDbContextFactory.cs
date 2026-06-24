using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDBContext>
{
    public ApplicationDBContext CreateDbContext(string[] args)
    {
        var connectionString =
            Environment.GetEnvironmentVariable("DIAGRAM_CONNECTION_STRING");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new Exception("DIAGRAM_CONNECTION_STRING is missing");
        }

        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDBContext>();

        optionsBuilder.UseNpgsql(connectionString);

        return new ApplicationDBContext(optionsBuilder.Options);
    }
}