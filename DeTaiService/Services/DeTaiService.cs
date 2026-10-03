
using Microsoft.EntityFrameworkCore;
using DeTaiService.Data;
using DeTaiService.Models;

namespace DeTaiService.Services;

public class DeTaiService : IDeTaiService
{
    private readonly AppDbContext _context;

    public DeTaiService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<DeTai>> GetAllAsync()
    {
        return await _context.DeTais
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<DeTai?> GetByIdAsync(string maDT)
    {
        return await _context.DeTais
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.MaDT == maDT);
    }

    public async Task<bool> CreateAsync(DeTai deTai)
    {
        bool exists = await _context.DeTais
            .AnyAsync(d => d.MaDT == deTai.MaDT);

        if (exists)
            return false;

        _context.DeTais.Add(deTai);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> UpdateAsync(string maDT, DeTai deTai)
    {
        var existing = await _context.DeTais
            .FirstOrDefaultAsync(d => d.MaDT == maDT);

        if (existing == null)
            return false;

        existing.TenDT = deTai.TenDT;
        existing.MoTa = deTai.MoTa;
        existing.GiangVienHD = deTai.GiangVienHD;
        existing.SoLuongToiDa = deTai.SoLuongToiDa;
        existing.TrangThai = deTai.TrangThai;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(string maDT)
    {
        var deTai = await _context.DeTais
            .FirstOrDefaultAsync(d => d.MaDT == maDT);

        if (deTai == null)
            return false;

        _context.DeTais.Remove(deTai);
        await _context.SaveChangesAsync();

        return true;
    }
}