
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DangKyService.Data;
using DangKyService.Models;
using DangKyService.Services;

namespace DangKyService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DangKyController : ControllerBase
{
    private readonly IDangKyService _service;

    public DangKyController(IDangKyService service)
    {
        _service = service;
    }

    // GET: api/DangKy
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();
        return Ok(result);
    }

    // GET: api/DangKy/1
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var dangKy = await _service.GetByIdAsync(id);

        if (dangKy == null)
            return NotFound(new { message = "Không tìm thấy đăng ký" });

        return Ok(dangKy);
    }

    // POST: api/DangKy
    [HttpPost]
    public async Task<IActionResult> Create(DangKyRequest request)
    {
        try
        {
            var (data, error) = await _service.CreateAsync(request);

            if (error != null)
                return BadRequest(new { message = error });

            return CreatedAtAction(
                nameof(GetById),
                new { id = data!.MaDK },
                data
            );
        }
        catch (DbUpdateException)
        {
            return Conflict(new
            {
                message = "Đăng ký bị trùng hoặc vi phạm ràng buộc dữ liệu"
            });
        }
    }

    // PUT: api/DangKy/1/trangthai
    [HttpPut("{id:int}/trangthai")]
    public async Task<IActionResult> UpdateStatus(
        int id, TrangThaiRequest request)
    {
        var (data, error) = await _service.UpdateStatusAsync(
            id, request.TrangThai);

        if (error != null)
            return BadRequest(new { message = error });

        return Ok(data);
    }

    // PUT: api/DangKy/1/diem
    [HttpPut("{id:int}/diem")]
    public async Task<IActionResult> UpdateScore(
        int id, DiemRequest request)
    {
        var (data, error) = await _service.UpdateScoreAsync(
            id, request.Diem);

        if (error != null)
            return BadRequest(new { message = error });

        return Ok(data);
    }

    // DELETE: api/DangKy/1
    // Hủy đăng ký, vẫn giữ lại bản ghi để theo dõi
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Cancel(int id)
    {
        var (success, error) = await _service.CancelAsync(id);

        if (!success)
            return NotFound(new { message = error });

        return Ok(new { message = "Đã hủy đăng ký" });
    }
}