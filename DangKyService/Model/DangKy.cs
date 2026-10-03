
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DangKyService.Models;

[Table("DANGKY")]
public class DangKy
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int MaDK { get; set; }

    [Required]
    [StringLength(20)]
    public string MaSV { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string MaDT { get; set; } = string.Empty;

    public DateTime NgayDangKy { get; set; } = DateTime.Now;

    [StringLength(30)]
    public string TrangThai { get; set; } = "ChoDuyet";

    [Range(0, 10)]
    public decimal? Diem { get; set; }
}