
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DeTaiService.Models;

[Table("DETAI")]
public class DeTai
{
    [Key]
    [StringLength(20)]
    public string MaDT { get; set; } = string.Empty;

    [Required]
    [StringLength(200)]
    public string TenDT { get; set; } = string.Empty;

    public string? MoTa { get; set; }

    [StringLength(100)]
    public string? GiangVienHD { get; set; }

    [Range(1, 100)]
    public int SoLuongToiDa { get; set; } = 1;

    [StringLength(30)]
    public string TrangThai { get; set; } = "DangMo";
}