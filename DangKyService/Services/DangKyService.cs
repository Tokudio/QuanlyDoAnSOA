
using System.Net;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using DangKyService.Data;
using DangKyService.Models;

namespace DangKyService.Services;

public class DangKyService : IDangKyService
{
    private readonly AppDbContext _context;
    private readonly IHttpClientFactory _httpClientFactory;

    public DangKyService(
        AppDbContext context,
        IHttpClientFactory httpClientFactory)
    {
        _context = context;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<List<DangKy>> GetAllAsync()
    {
        return await _context.DangKys
            .AsNoTracking()
            .OrderByDescending(d => d.NgayDangKy)
            .ToListAsync();
    }

    public async Task<DangKy?> GetByIdAsync(int maDK)
    {
        return await _context.DangKys
            .AsNoTracking()
            .FirstOrDefaultAsync(d => d.MaDK == maDK);
    }

    public async Task<(DangKy? Data, string? Error)> CreateAsync(
        DangKyRequest request)
    {
        // Kiểm tra sinh viên thông qua SinhVienService
        var sinhVienClient =
            _httpClientFactory.CreateClient("SinhVienApi");

        try
        {
            var studentResponse = await sinhVienClient.GetAsync(
                $"api/SinhVien/{Uri.EscapeDataString(request.MaSV)}");

            if (studentResponse.StatusCode == HttpStatusCode.NotFound)
                return (null, "Không tìm thấy sinh viên");

            if (!studentResponse.IsSuccessStatusCode)
                return (null, "SinhVienService đang gặp lỗi");

            // Kiểm tra đề tài thông qua DeTaiService
            var deTaiClient =
                _httpClientFactory.CreateClient("DeTaiApi");

            var topicResponse = await deTaiClient.GetAsync(
                $"api/DeTai/{Uri.EscapeDataString(request.MaDT)}");

            if (topicResponse.StatusCode == HttpStatusCode.NotFound)
                return (null, "Không tìm thấy đề tài");

            if (!topicResponse.IsSuccessStatusCode)
                return (null, "DeTaiService đang gặp lỗi");

            var deTai = await topicResponse.Content
                .ReadFromJsonAsync<DeTaiApiDto>();

            if (deTai == null)
                return (null, "Không đọc được thông tin đề tài");

            if (deTai.TrangThai != "DangMo")
                return (null, "Đề tài hiện không mở đăng ký");

            // Một sinh viên chỉ đăng ký một đề tài
            bool daDangKy = await _context.DangKys
                .AnyAsync(d => d.MaSV == request.MaSV);

            if (daDangKy)
                return (null, "Sinh viên đã có đăng ký");

            // Tính số đăng ký đang chiếm chỗ
            int soLuongDangKy = await _context.DangKys
                .CountAsync(d =>
                    d.MaDT == request.MaDT &&
                    (d.TrangThai == "ChoDuyet" ||
                     d.TrangThai == "DaDuyet"));

            if (soLuongDangKy >= deTai.SoLuongToiDa)
                return (null, "Đề tài đã đủ số lượng đăng ký");

            var dangKy = new DangKy
            {
                MaSV = request.MaSV,
                MaDT = request.MaDT,
                NgayDangKy = DateTime.Now,
                TrangThai = "ChoDuyet",
                Diem = null
            };

            _context.DangKys.Add(dangKy);
            await _context.SaveChangesAsync();

            return (dangKy, null);
        }
        catch (HttpRequestException)
        {
            return (null, "Không thể kết nối tới service sinh viên hoặc đề tài");
        }
    }

    public async Task<(DangKy? Data, string? Error)> UpdateStatusAsync(
        int maDK, string trangThai)
    {
        var dangKy = await _context.DangKys
            .FirstOrDefaultAsync(d => d.MaDK == maDK);

        if (dangKy == null)
            return (null, "Không tìm thấy đăng ký");

        if (dangKy.TrangThai == "DaHuy")
            return (null, "Đăng ký đã bị hủy");

        if (trangThai != "DaDuyet" && trangThai != "TuChoi")
            return (null, "Trạng thái chỉ được là DaDuyet hoặc TuChoi");

        if (trangThai == "DaDuyet")
        {
            var client = _httpClientFactory.CreateClient("DeTaiApi");

            try
            {
                var response = await client.GetAsync(
                    $"api/DeTai/{Uri.EscapeDataString(dangKy.MaDT)}");

                if (!response.IsSuccessStatusCode)
                    return (null, "Không lấy được thông tin đề tài");

                var deTai = await response.Content
                    .ReadFromJsonAsync<DeTaiApiDto>();

                if (deTai == null || deTai.TrangThai != "DangMo")
                    return (null, "Đề tài không còn mở");

                int soDangKyKhac = await _context.DangKys
                    .CountAsync(d =>
                        d.MaDT == dangKy.MaDT &&
                        d.MaDK != maDK &&
                        (d.TrangThai == "ChoDuyet" ||
                         d.TrangThai == "DaDuyet"));

                if (soDangKyKhac >= deTai.SoLuongToiDa)
                    return (null, "Đề tài đã đủ số lượng");
            }
            catch (HttpRequestException)
            {
                return (null, "Không thể kết nối DeTaiService");
            }
        }

        dangKy.TrangThai = trangThai;
        await _context.SaveChangesAsync();

        return (dangKy, null);
    }

    public async Task<(DangKy? Data, string? Error)> UpdateScoreAsync(
        int maDK, decimal diem)
    {
        var dangKy = await _context.DangKys
            .FirstOrDefaultAsync(d => d.MaDK == maDK);

        if (dangKy == null)
            return (null, "Không tìm thấy đăng ký");

        if (dangKy.TrangThai != "DaDuyet")
            return (null, "Chỉ được chấm điểm đăng ký đã duyệt");

        if (diem < 0 || diem > 10)
            return (null, "Điểm phải nằm trong khoảng từ 0 đến 10");

        dangKy.Diem = diem;
        await _context.SaveChangesAsync();

        return (dangKy, null);
    }

    public async Task<(bool Success, string? Error)> CancelAsync(int maDK)
    {
        var dangKy = await _context.DangKys
            .FirstOrDefaultAsync(d => d.MaDK == maDK);

        if (dangKy == null)
            return (false, "Không tìm thấy đăng ký");

        if (dangKy.TrangThai == "DaHuy")
            return (false, "Đăng ký đã bị hủy");

        dangKy.TrangThai = "DaHuy";
        await _context.SaveChangesAsync();

        return (true, null);
    }
}