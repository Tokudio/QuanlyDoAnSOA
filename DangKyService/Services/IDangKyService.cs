
using DangKyService.Models;

namespace DangKyService.Services;

public interface IDangKyService
{
    Task<List<DangKy>> GetAllAsync();
    Task<DangKy?> GetByIdAsync(int maDK);

    Task<(DangKy? Data, string? Error)> CreateAsync(
        DangKyRequest request);

    Task<(DangKy? Data, string? Error)> UpdateStatusAsync(
        int maDK, string trangThai);

    Task<(DangKy? Data, string? Error)> UpdateScoreAsync(
        int maDK, decimal diem);

    Task<(bool Success, string? Error)> CancelAsync(int maDK);
}