
using Microsoft.EntityFrameworkCore;
using DangKyService.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace DangKyService.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
    public DbSet<DangKy> DangKys => Set<DangKy>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<DangKy>(entity =>
        {
            entity.ToTable("DANGKY");

            entity.HasKey(d => d.MaDK);

            entity.Property(d => d.MaDK)
                .ValueGeneratedOnAdd();

            entity.Property(d => d.MaSV)
                .HasMaxLength(20)
                .IsRequired();

            entity.Property(d => d.MaDT)
                .HasMaxLength(20)
                .IsRequired();

            entity.Property(d => d.NgayDangKy)
                .HasColumnType("datetime2")
                .HasDefaultValueSql("SYSDATETIME()");

            entity.Property(d => d.TrangThai)
                .HasMaxLength(30)
                .HasDefaultValue("ChoDuyet");

            entity.Property(d => d.Diem)
                .HasColumnType("decimal(4,2)");

            // Mỗi sinh viên chỉ được đăng ký một đề tài
            entity.HasIndex(d => d.MaSV)
                .IsUnique();
        });
    }
}