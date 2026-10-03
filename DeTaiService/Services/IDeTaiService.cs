
using DeTaiService.Models;

namespace DeTaiService.Services;

public interface IDeTaiService
{
    Task<List<DeTai>> GetAllAsync();
    Task<DeTai?> GetByIdAsync(string maDT);
    Task<bool> CreateAsync(DeTai deTai);
    Task<bool> UpdateAsync(string maDT, DeTai deTai);
    Task<bool> DeleteAsync(string maDT);
}