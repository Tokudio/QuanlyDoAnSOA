
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SinhVienService.Models;

[Table("SINHVIEN")]
public class SinhVien
{
    [Key]
    [StringLength(20)]
    public string MaSV { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string HoTen { get; set; } = string.Empty;

    public DateTime? NgaySinh { get; set; }

    [StringLength(100)]
    public string? Email { get; set; }

    [StringLength(50)]
    public string? Lop { get; set; }
}