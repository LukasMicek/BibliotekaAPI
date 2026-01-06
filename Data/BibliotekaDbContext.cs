using BibliotekaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BibliotekaAPI.Data;

public class BibliotekaDbContext : DbContext
{
    public BibliotekaDbContext(DbContextOptions<BibliotekaDbContext> options) : base(options) { }

    public DbSet<Autor> Authors => Set<Autor>();
    public DbSet<Książka> Books => Set<Książka>();
    public DbSet<Egzemplarz> Copies => Set<Egzemplarz>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Autor>()
            .HasMany(a => a.Books)
            .WithOne(b => b.Author!)
            .HasForeignKey(b => b.AuthorId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Książka>()
            .HasMany(b => b.Copies)
            .WithOne(c => c.Book!)
            .HasForeignKey(c => c.BookId)
            .OnDelete(DeleteBehavior.Cascade);

        base.OnModelCreating(modelBuilder);
    }
}

