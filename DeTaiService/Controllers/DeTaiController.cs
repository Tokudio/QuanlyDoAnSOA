
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DeTaiService.Models;
using DeTaiService.Services;

namespace DeTaiService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DeTaiController : ControllerBase
{
    private readonly IDeTaiService _service;

    public DeTaiController(IDeTaiService service)
    {
        _service = service;
    }

    // GET: api/DeTai
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _service.GetAllAsync();
        return Ok(result);
    }

    // GET: api/DeTai/DT001
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var deTai = await _service.GetByIdAsync(id);

        if (deTai == null)
            return NotFound(new { message = "Không tìm thấy đề tài" });

        return Ok(deTai);
    }

    // POST: api/DeTai
    [HttpPost]
    public async Task<IActionResult> Create(DeTai deTai)
    {
        bool created = await _service.CreateAsync(deTai);

        if (!created)
            return Conflict(new { message = "Mã đề tài đã tồn tại" });

        return CreatedAtAction(
            nameof(GetById),
            new { id = deTai.MaDT },
            deTai
        );
    }

    // PUT: api/DeTai/DT001
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, DeTai deTai)
    {
        if (id != deTai.MaDT)
            return BadRequest(new { message = "Mã đề tài không khớp" });

        bool updated = await _service.UpdateAsync(id, deTai);

        if (!updated)
            return NotFound(new { message = "Không tìm thấy đề tài" });

        return Ok(new { message = "Cập nhật đề tài thành công" });
    }

    // DELETE: api/DeTai/DT001
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            bool deleted = await _service.DeleteAsync(id);

            if (!deleted)
                return NotFound(new { message = "Không tìm thấy đề tài" });

            return NoContent();
        }
        catch (DbUpdateException)
        {
            return Conflict(new
            {
                message = "Không thể xóa đề tài đang có đăng ký"
            });
        }
    }
}