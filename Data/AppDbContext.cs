using Microsoft.EntityFrameworkCore;
using erooms.Models;

namespace erooms.Data
{
    //up
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Room> Rooms { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Room>().HasData(
                new Room { Id = 1, Name = "Audit", Capacity = 30, Location = "Gedung Pasca", IsAvailable = true },
                new Room { Id = 2, Name = "B202", Capacity = 15, Location = "Gedung D4", IsAvailable = true },
                new Room { Id = 3, Name = "HH105", Capacity = 100, Location = "Gedung D3", IsAvailable = false }
            );
        }
    }
}
