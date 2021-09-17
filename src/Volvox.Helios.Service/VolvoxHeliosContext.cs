using Microsoft.EntityFrameworkCore;

namespace Volvox.Helios.Service
{
    public class VolvoxHeliosContext : DbContext
    {
        public VolvoxHeliosContext(DbContextOptions<VolvoxHeliosContext> options)
            : base(options)
        {
        }
    }
}