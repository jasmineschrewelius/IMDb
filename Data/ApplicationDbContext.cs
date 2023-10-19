using System.Diagnostics;
using IMDb.Domain;
using Microsoft.EntityFrameworkCore;

namespace IMDb.Data;

class ApplicationDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Server=.;Database=IMDb;Trusted_Connection=True;Encrypt=False");
        optionsBuilder.LogTo(x => Debug.WriteLine(x));
    }

    public DbSet<Movie> Movie { get; set; }
    public DbSet<Actor> Actor { get; set; }
}