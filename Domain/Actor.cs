using System.ComponentModel.DataAnnotations;

namespace IMDb.Domain;

class Actor
{
    public int Id { get; set; }

    [MaxLength(50)]
    public required string FirstName { get; set; }

    [MaxLength(50)]
    public required string LastName { get; set; }

    public required DateTime BirthDate { get; set; }

    public DateTime? DeceasedDate { get; set; }

    public bool IsDeceased => DeceasedDate is not null;

    public int Age
    {
        get 
        {
            var x = IsDeceased ? DeceasedDate : DateTime.Now;

            var age = x - BirthDate;

            return (int) (age.Value.TotalDays / 365.25);
        }
    }

    // Navigration property
    public ICollection<Movie> Movies { get; set; } = new List<Movie>();
}