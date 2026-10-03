
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SinhVienService.Models;
using SinhVienService.Services;

namespace SinhVienService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SinhVienController : ControllerBase
{
    private readonly ISinhVienService _service;

    public SinhVienController(ISinhVienService service)
    {
        _service = service;
    }

    // GET: api/SinhVien
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();
        return Ok(result);
    }

    // GET: api/SinhVien/SV001
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var result = await _service.GetByIdAsync(id);

        if (result == null)
            return NotFound(new { message = "Khong tim thay sinh vien" });

        return Ok(result);
    }

    // POST: api/SinhVien
    [HttpPost]
    public async Task<IActionResult> Create(SinhVien sinhVien)
    {
        bool created = await _service.CreateAsync(sinhVien);

        if (!created)
            return Conflict(new { message = "Ma sinh vien da ton tai" });

        return CreatedAtAction(
            nameof(GetById),
            new { id = sinhVien.MaSV },
            sinhVien
        );
    }

    // PUT: api/SinhVien/SV001
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        string id, SinhVien sinhVien)
    {
        bool updated = await _service.UpdateAsync(id, sinhVien);

        if (!updated)
            return NotFound(new { message = "Khong tim thay sinh vien" });

        return Ok(new { message = "Cap nhat thanh cong" });
    }

    // DELETE: api/SinhVien/SV001
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            bool deleted = await _service.DeleteAsync(id);

            if (!deleted)
                return NotFound(new { message = "Khong tim thay sinh vien" });

            return NoContent();
        }
        catch (DbUpdateException)
        {
            return Conflict(new
            {
                message = "Khong the xoa sinh vien da co dang ky"
            });
        }
    }
}