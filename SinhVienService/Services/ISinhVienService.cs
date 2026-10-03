
using SinhVienService.Models;

namespace SinhVienService.Services;

public interface ISinhVienService
{
    Task<List<SinhVien>> GetAllAsync();

    Task<SinhVien?> GetByIdAsync(string maSV);

    Task<bool> CreateAsync(SinhVien sinhVien);

    Task<bool> UpdateAsync(string maSV, SinhVien sinhVien);

    Task<bool> DeleteAsync(string maSV);
}