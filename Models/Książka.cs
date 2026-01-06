namespace BibliotekaAPI.Models;

public class Książka
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public int Year { get; set; }

    public int AuthorId { get; set; }
    public Autor? Author { get; set; }

    public List<Egzemplarz> Copies { get; set; } = new();
}

