using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SportGo.Repository.Entities;
using SportGo.Repository.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportGo.Repository
{
    public class SportGoDbContext : DbContext
    {
        // Constructor này nhận các tùy chọn cấu hình (như chuỗi kết nối) từ bên ngoài (thường là từ Program.cs)
        public SportGoDbContext(DbContextOptions<SportGoDbContext> options) : base(options)
        {
        }

        // Khai báo các bảng trong cơ sở dữ liệu
        // Mỗi DbSet<T> tương ứng với một bảng
        public DbSet<User> Users { get; set; }
        public DbSet<SportType> SportTypes { get; set; }
        public DbSet<Facility> Facilities { get; set; }
        public DbSet<Court> Courts { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<UserPackage> UserPackages { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<UserRefreshToken> UserRefreshTokens { get; set; }
        public DbSet<ProviderProfile> ProviderProfiles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Email).IsUnique();
                entity.HasIndex(e => e.PhoneNumber).IsUnique();
            });
            modelBuilder.Entity<User>()
                .HasOne(u => u.ProviderProfile)
                .WithOne(p => p.User)
                .HasForeignKey<ProviderProfile>(p => p.UserId);

            modelBuilder.Entity<Facility>()
                .HasOne(f => f.Provider)
                .WithMany(u => u.Facilities)
                .HasForeignKey(f => f.ProviderId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<UserPackage>()
                .HasOne(up => up.User)
                .WithMany(u => u.UserPackages)
                .HasForeignKey(up => up.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.UserPackage)
                .WithMany(up => up.Bookings)
                .HasForeignKey(b => b.UserPackageId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserRefreshToken>(entity =>
            {
                entity.HasIndex(e => e.Token).IsUnique();
            });
            modelBuilder.Entity<User>()
            .Property(u => u.ProviderStatus)
            .HasConversion(new EnumToStringConverter<ProviderStatus>());
            modelBuilder.Entity<User>().Property(u => u.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            modelBuilder.Entity<Facility>().Property(f => f.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            modelBuilder.Entity<Booking>().Property(b => b.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
            modelBuilder.Entity<Payment>().Property(p => p.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
