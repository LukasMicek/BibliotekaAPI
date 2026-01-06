using BibliotekaAPI.Data;
using BibliotekaAPI.Dtos;
using BibliotekaAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BibliotekaAPI.Controllers;

[ApiController]
[Route("books")]
public class KsiążkasController : ControllerBase
{
    private readonly BibliotekaDbContext _db;

    public KsiążkasController(BibliotekaDbContext db) => _db = db;

    [HttpGet]
    public async Task<ActionResult<List<KsiążkaDto>>> GetAll([FromQuery] int? authorId)
    {
        var query = _db.Books.Include(b => b.Author).AsQueryable();

        if (authorId.HasValue)
            query = query.Where(b => b.AuthorId == authorId.Value); // GET /books?authorId=... :contentReference[oaicite:18]{index=18}

        var books = await query
            .OrderBy(b => b.Id)
            .Select(b => new KsiążkaDto(
                b.Id,
                b.Title,
                b.Year,
                new AutorDto(b.Author!.Id, b.Author.FirstName, b.Author.LastName)
            ))
            .ToListAsync();

        return Ok(books);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<KsiążkaDto>> GetById(int id)
    {
        var b = await _db.Books.Include(x => x.Author).FirstOrDefaultAsync(x => x.Id == id);
        if (b is null) return NotFound();

        return Ok(new KsiążkaDto(
            b.Id,
            b.Title,
            b.Year,
            new AutorDto(b.Author!.Id, b.Author.FirstName, b.Author.LastName)
        ));
    }

    [HttpPost]
    public async Task<ActionResult<KsiążkaDto>> Create([FromBody] KsiążkaCreateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Title)) return BadRequest();
        if (dto.Year < 0) return BadRequest();
        var author = await _db.Authors.FirstOrDefaultAsync(a => a.Id == dto.AuthorId);
        if (author is null) return BadRequest(); // authorId musi istnieć 

        var book = new Książka
        {
            Title = dto.Title.Trim(),
            Year = dto.Year,
            AuthorId = author.Id
        };

        _db.Books.Add(book);
        await _db.SaveChangesAsync();

        var result = new KsiążkaDto(
            book.Id,
            book.Title,
            book.Year,
            new AutorDto(author.Id, author.FirstName, author.LastName)
        );

        return Created($"/books/{book.Id}", result); // 201 :contentReference[oaicite:20]{index=20}
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] KsiążkaUpdateDto dto)
    {
        if (dto.Id != id) return BadRequest();
        if (string.IsNullOrWhiteSpace(dto.Title)) return BadRequest();
        if (dto.Year < 0) return BadRequest();

        var book = await _db.Books.FirstOrDefaultAsync(b => b.Id == id);
        if (book is null) return NotFound(); // :contentReference[oaicite:21]{index=21}

        var author = await _db.Authors.FirstOrDefaultAsync(a => a.Id == dto.AuthorId);
        if (author is null) return BadRequest();

        book.Title = dto.Title.Trim();
        book.Year = dto.Year;
        book.AuthorId = author.Id;

        await _db.SaveChangesAsync();
        return NoContent(); // 204 :contentReference[oaicite:22]{index=22}
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var book = await _db.Books.FirstOrDefaultAsync(b => b.Id == id);
        if (book is null) return NotFound();

        _db.Books.Remove(book);
        await _db.SaveChangesAsync();
        return NoContent(); // 204 :contentReference[oaicite:23]{index=23}
    }
}
