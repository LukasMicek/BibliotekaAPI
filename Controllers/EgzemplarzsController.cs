using BibliotekaAPI.Data;
using BibliotekaAPI.Dtos;
using BibliotekaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BibliotekaAPI.Controllers;

[ApiController]
[Route("copies")]
[Route("exemplars")]
public class CopiesController : ControllerBase
{
    private readonly BibliotekaDbContext _db;
    public CopiesController(BibliotekaDbContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<List<EgzemplarzDto>>> GetAll([FromQuery] int? bookId)
    {
        var q = _db.Copies.AsQueryable();

        if (bookId.HasValue)
            q = q.Where(c => c.BookId == bookId.Value);

        var result = await q
            .OrderBy(c => c.Id)
            .Select(c => new EgzemplarzDto(c.Id, c.BookId))
            .ToListAsync();

        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<EgzemplarzDto>> GetById(int id)
    {
        var c = await _db.Copies.FirstOrDefaultAsync(x => x.Id == id);
        if (c is null) return NotFound();

        return Ok(new EgzemplarzDto(c.Id, c.BookId));
    }

    [HttpPost]
    public async Task<ActionResult<EgzemplarzDto>> Create([FromBody] EgzemplarzCreateDto dto)
    {
        if (dto.BookId <= 0) return BadRequest();

        var bookExists = await _db.Books.AnyAsync(b => b.Id == dto.BookId);
        if (!bookExists) return BadRequest();

        var copy = new Egzemplarz { BookId = dto.BookId };
        _db.Copies.Add(copy);
        await _db.SaveChangesAsync();

        var result = new EgzemplarzDto(copy.Id, copy.BookId);
        return Created($"/copies/{copy.Id}", result);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] EgzemplarzUpdateDto dto)
    {
        if (dto.Id != id) return BadRequest();
        if (dto.BookId <= 0) return BadRequest();

        var copy = await _db.Copies.FirstOrDefaultAsync(x => x.Id == id);
        if (copy is null) return NotFound();

        var bookExists = await _db.Books.AnyAsync(b => b.Id == dto.BookId);
        if (!bookExists) return BadRequest();

        copy.BookId = dto.BookId;
        await _db.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var copy = await _db.Copies.FirstOrDefaultAsync(x => x.Id == id);
        if (copy is null) return NotFound();

        _db.Copies.Remove(copy);
        await _db.SaveChangesAsync();

        return NoContent();
    }
}
