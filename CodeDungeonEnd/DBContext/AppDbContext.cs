using CodeDungeon.Enums;
using CodeDungeon.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

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
            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasConversion<string>();


            modelBuilder.Entity<User>(entity =>
            {
               
                entity.HasIndex(u => u.Username).IsUnique();
                entity.HasIndex(u => u.Email).IsUnique();

     
                entity.Property(u => u.FinCode).HasMaxLength(7).IsFixedLength();
            });

            var adminId = Guid.Parse("A1B2C3D4-E5F6-7890-ABCD-EF1234567890");

            modelBuilder.Entity<User>().HasData(new User
            {
                Id = adminId,
                Username = "superadmin",
                Name = "Super",
                Surname = "Admin",
                FatherName = "System",
                Email = "admin@formix.com",
                FinCode = "ADMIN12", 
                PhoneNumber = "+994000000000",
                BirthDate = new DateTime(1990, 1, 1).ToUniversalTime(),
                Role = UserRole.SuperAdmin, 
                IsPasswordConfirmed = false, 
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            });
        }

    }
}