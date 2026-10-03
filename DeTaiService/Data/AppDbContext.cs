
using Microsoft.EntityFrameworkCore;
using DeTaiService.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace DeTaiService.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
    public DbSet<DeTai> DeTais => Set<DeTai>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<DeTai>(entity =>
        {
            entity.ToTable("DETAI");

            entity.HasKey(d => d.MaDT);

            entity.Property(d => d.MaDT)
                .HasMaxLength(20)
                .IsRequired();

            entity.Property(d => d.TenDT)
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(d => d.MoTa)
                .HasColumnType("nvarchar(max)");

            entity.Property(d => d.GiangVienHD)
                .HasMaxLength(100);

            entity.Property(d => d.SoLuongToiDa)
                .HasDefaultValue(1)
                .IsRequired();

            entity.Property(d => d.TrangThai)
                .HasMaxLength(30)
                .HasDefaultValue("DangMo");
        });
    }
}