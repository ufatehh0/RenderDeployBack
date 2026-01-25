using Microsoft.EntityFrameworkCore;
using CodeDungeonAPI.Models;

namespace CodeDungeonAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<UserInfo> UsersInfo { get; set; }
        public DbSet<UserCharacter> UserCharacters { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 1. Cədvəl adlarını kiçik hərflərlə bazaya uyğunlaşdırırıq
            modelBuilder.Entity<User>().ToTable("users");
            modelBuilder.Entity<UserInfo>().ToTable("users_info");
            modelBuilder.Entity<UserCharacter>().ToTable("user_characters");

            // 2. User <-> UserInfo (One-to-One) əlaqəsi
            modelBuilder.Entity<User>()
                .HasOne(u => u.UserInfo)
                .WithOne(ui => ui.User)
                .HasForeignKey<UserInfo>(ui => ui.Id);

            // 3. User <-> UserCharacter (One-to-One) əlaqəsi
            // Bu hissə mütləqdir ki, xarakter məlumatları düzgün Id ilə yazılsın
            modelBuilder.Entity<User>()
                .HasOne(u => u.UserCharacter)
                .WithOne(uc => uc.User)
                .HasForeignKey<UserCharacter>(uc => uc.Id);
        }
    }
}