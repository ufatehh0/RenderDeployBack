using Microsoft.EntityFrameworkCore;
using CodeDungeonAPI.Models;

namespace CodeDungeonAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        // 1. Yeni cədvəli DbSet olaraq əlavə edirik
        public DbSet<UserInfo> UsersInfo { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 2. User cədvəlinin adını təyin edirik
            modelBuilder.Entity<User>().ToTable("users");

            // 3. UserInfo cədvəlinin adını təyin edirik
            modelBuilder.Entity<UserInfo>().ToTable("users_info");

            // 4. One-to-One əlaqəsini Fluent API ilə dəqiqləşdiririk
            modelBuilder.Entity<User>()
                .HasOne(u => u.UserInfo) // User-in bir dənə UserInfo-su var
                .WithOne(ui => ui.User)  // UserInfo-nun bir dənə User-i var
                .HasForeignKey<UserInfo>(ui => ui.Id); // Xarici açar UserInfo-dakı Id-dir
        }
    }
}