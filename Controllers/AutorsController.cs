using BibliotekaAPI.Data;
using BibliotekaAPI.Dtos;
using BibliotekaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BibliotekaAPI.Controllers;

[ApiController]
[Route("authors")]
public class AutorsController : ControllerBase
{
    private readonly BibliotekaDbContext _db;

    public AutorsController(BibliotekaDbContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<List<AutorDto>>> GetAll()
    {
        var authors = await _db.Authors
            .OrderBy(a => a.Id)
            .Select(a => new AutorDto(a.Id, a.FirstName, a.LastName))
            .ToListAsync();

        return Ok(authors);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<AutorDto>> GetById(int id)
    {
        var a = await _db.Authors.FirstOrDefaultAsync(x => x.Id == id);
        if (a is null) return NotFound();

        return Ok(new AutorDto(a.Id, a.FirstName, a.LastName));
    }

    [HttpPost]
    public async Task<ActionResult<AutorDto>> Create([FromBody] AutorCreateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.FirstName) || string.IsNullOrWhiteSpace(dto.LastName))
            return BadRequest();

        var author = new Autor
        {
            FirstName = dto.FirstName.Trim(),
            LastName = dto.LastName.Trim()
        };

        _db.Authors.Add(author);
        await _db.SaveChangesAsync();

        var result = new AutorDto(author.Id, author.FirstName, author.LastName);
        return Created($"/authors/{author.Id}", result); // 201 :contentReference[oaicite:15]{index=15}
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] AutorUpdateDto dto)
    {
        if (dto.Id != id) return BadRequest();
        if (string.IsNullOrWhiteSpace(dto.FirstName) || string.IsNullOrWhiteSpace(dto.LastName))
            return BadRequest();

        var author = await _db.Authors.FirstOrDefaultAsync(x => x.Id == id);
        if (author is null) return NotFound();

        author.FirstName = dto.FirstName.Trim();
        author.LastName = dto.LastName.Trim();

        await _db.SaveChangesAsync();
        return NoContent(); // 204 :contentReference[oaicite:16]{index=16}
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var author = await _db.Authors.FirstOrDefaultAsync(x => x.Id == id);
        if (author is null) return NotFound();

        _db.Authors.Remove(author);
        await _db.SaveChangesAsync();
        return NoContent(); // 204 :contentReference[oaicite:17]{index=17}
    }
}
