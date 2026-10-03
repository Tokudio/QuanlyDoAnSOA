
using System.ComponentModel.DataAnnotations;

namespace DangKyService.Models;

public class DangKyRequest
{
    [Required]
    [StringLength(20)]
    public string MaSV { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string MaDT { get; set; } = string.Empty;
}

public class TrangThaiRequest
{
    [Required]
    public string TrangThai { get; set; } = string.Empty;
}

public class DiemRequest
{
    [Required]
    [Range(typeof(decimal), "0", "10")]
    public decimal Diem { get; set; }
}

// Dữ liệu cơ bản nhận từ DeTaiService
public class DeTaiApiDto
{
    public string MaDT { get; set; } = string.Empty;
    public string TenDT { get; set; } = string.Empty;
    public int SoLuongToiDa { get; set; }
    public string TrangThai { get; set; } = string.Empty;
}