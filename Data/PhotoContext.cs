using Microsoft.EntityFrameworkCore;
using PortraitsByPerminder.Model;

namespace PortraitsByPerminder.Data
{
    public class PhotoContext : DbContext
    {
        public PhotoContext(DbContextOptions<PhotoContext> options) : base(options)
        {
        }

        public DbSet<Photo> Photos { get; set;}


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure any additional settings for your entities
        }
    }
}

