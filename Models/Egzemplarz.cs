namespace BibliotekaAPI.Models;

public class Egzemplarz
{
    public int Id { get; set; }

    public int BookId { get; set; }
    public Książka? Book { get; set; }
}
