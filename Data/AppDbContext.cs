using Microsoft.EntityFrameworkCore;
using CodeDungeonAPI.Models;

namespace CodeDungeonAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // PostgreSQL-də yaratdığın cədvəl adı kiçik hərflə olduğu üçün:
            modelBuilder.Entity<User>().ToTable("users");
        }
    }
}