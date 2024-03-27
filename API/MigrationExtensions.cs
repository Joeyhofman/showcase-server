using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Writers;

namespace API
{
    public static class MigrationExtensions
    {
        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using IServiceScope scope = app.ApplicationServices.CreateScope();
            using ApplicationDBContext context =  scope.ServiceProvider.GetRequiredService<ApplicationDBContext>();
            context.Database.Migrate();
        }
    }
}
