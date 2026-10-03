
using Microsoft.EntityFrameworkCore;
using SinhVienService.Models;

namespace SinhVienService.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<SinhVien> SinhViens => Set<SinhVien>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<SinhVien>(entity =>
        {
            entity.HasKey(s => s.MaSV);

            entity.Property(s => s.MaSV)
                .HasColumnName("MaSV")
                .HasMaxLength(20)
                .IsRequired();

            entity.Property(s => s.HoTen)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(s => s.NgaySinh)
                .HasColumnType("date");

            entity.Property(s => s.Email)
                .HasMaxLength(100);

            entity.Property(s => s.Lop)
                .HasMaxLength(50);
        });
    }
}