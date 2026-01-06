namespace BibliotekaAPI.Models
{
    public class Autor
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";

        public List<Książka> Books { get; set; } = new();
    }
}
