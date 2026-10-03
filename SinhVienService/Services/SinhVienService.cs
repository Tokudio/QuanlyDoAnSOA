
using Microsoft.EntityFrameworkCore;
using SinhVienService.Data;
using SinhVienService.Models;

namespace SinhVienService.Services;

public class SinhVienService : ISinhVienService
{
    private readonly AppDbContext _context;

    public SinhVienService(AppDbContext context)
    {
        _context = context;
    }

    // Lấy tất cả sinh viên
    public async Task<List<SinhVien>> GetAllAsync()
    {
        return await _context.SinhViens
            .AsNoTracking()
            .ToListAsync();
    }

    // Lấy sinh viên theo mã
    public async Task<SinhVien?> GetByIdAsync(string maSV)
    {
        return await _context.SinhViens
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.MaSV == maSV);
    }

    // Thêm sinh viên
    public async Task<bool> CreateAsync(SinhVien sinhVien)
    {
        bool exists = await _context.SinhViens
            .AnyAsync(s => s.MaSV == sinhVien.MaSV);

        if (exists)
            return false;

        _context.SinhViens.Add(sinhVien);

        await _context.SaveChangesAsync();

        return true;
    }

    // Cập nhật sinh viên
    public async Task<bool> UpdateAsync(
        string maSV, SinhVien sinhVien)
    {
        var existing = await _context.SinhViens
            .FirstOrDefaultAsync(s => s.MaSV == maSV);

        if (existing == null)
            return false;

        existing.HoTen = sinhVien.HoTen;
        existing.NgaySinh = sinhVien.NgaySinh;
        existing.Email = sinhVien.Email;
        existing.Lop = sinhVien.Lop;

        await _context.SaveChangesAsync();

        return true;
    }

    // Xóa sinh viên
    public async Task<bool> DeleteAsync(string maSV)
    {
        var sinhVien = await _context.SinhViens
            .FirstOrDefaultAsync(s => s.MaSV == maSV);

        if (sinhVien == null)
            return false;

        _context.SinhViens.Remove(sinhVien);

        await _context.SaveChangesAsync();

        return true;
    }
}