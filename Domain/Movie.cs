using System.ComponentModel.DataAnnotations;

namespace IMDb.Domain;

class Movie
{
    public int Id { get; set; }

    [MaxLength(50)]
    public required string Title { get; set; }

    [MaxLength(500)]
    public required string Plot { get; set; }

    [MaxLength(50)]
    public required string Genre { get; set; }

    [MaxLength(50)]
    public required string Director { get; set; }

    public required DateTime Premier { get; set; }
}