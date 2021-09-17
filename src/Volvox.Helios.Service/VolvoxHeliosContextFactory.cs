using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Volvox.Helios.Service
{
    public class VolvoxHeliosContextFactory : IDesignTimeDbContextFactory<VolvoxHeliosContext>
    {
        public VolvoxHeliosContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<VolvoxHeliosContext>();

            optionsBuilder.UseNpgsql("", options =>
                options.MigrationsAssembly("Volvox.Helios.Service"));

            return new VolvoxHeliosContext(optionsBuilder.Options);
        }
    }
}