using Microsoft.EntityFrameworkCore;

namespace ChallengeAndreani.Models
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options)
    :   base(options)
        {
        }
        public DbSet<Geolocalizacion> Geolocalizacion { get; set; }

    }
}

