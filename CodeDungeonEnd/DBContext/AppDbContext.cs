using CodeDungeon.Enums;
using CodeDungeon.Models.Entities.User;
using Microsoft.EntityFrameworkCore;

namespace CodeDungeon.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().Property(u => u.Role).HasConversion<string>();

            modelBuilder.Entity<User>(entity =>
            {
                entity.OwnsOne(u => u.Character);
                entity.HasIndex(u => u.Username).IsUnique();
                entity.HasIndex(u => u.Email).IsUnique();
            });

            var adminId = Guid.Parse("A1B2C3D4-E5F6-7890-ABCD-EF1234567890");

            // 1. ADIM: User verisini anonim nesne olarak ekle (Navigasyon hatasını önler)
            modelBuilder.Entity<User>().HasData(new
            {
                Id = adminId,
                Username = "admin",
                Name = "Super",
                Surname = "Admin",
                Email = "admin@formix.com",
                Level = 100,
                BirthDate = new DateTime(1990, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                Role = UserRole.SuperAdmin,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword("admin")
            });

            // 2. ADIM: Character verisini ekle
            modelBuilder.Entity<User>().OwnsOne(u => u.Character).HasData(new
            {
                UserId = adminId, // Eğer yine hata verirse burayı 'Id' yapmayı dene
                Gender = "None",
                Emotion = "Neutral",
                Clothing = "Default",
                HairColor = "None",
                Skin = "Default",
                ClothingColor = "None"
            });
        }
    }
}