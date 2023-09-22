using IMDb.Data;
using IMDb.Domain;
using static System.Console;

namespace IMDb;

class Program
{
  public static void Main()
  {
    Title = "IMDb v1.0";

    while (true)
    {
      WriteLine("1. Lägg till film");

      var keyPressed = ReadKey(true);

      switch (keyPressed.Key)
      {
        case ConsoleKey.D1:
        case ConsoleKey.NumPad1:

          AddMovieView();

          break;
      }

      Clear();
    }
  }

  private static void AddMovieView()
  {
    var title = GetUserInput("Titel");
    var plot = GetUserInput("Handling");
    var genre = GetUserInput("Genre");
    var director = GetUserInput("Regissör");
    var premier = DateTime.Parse(GetUserInput("Premiär (YYYY-MM-DD)"));

    var movie = new Movie
    {
      Title = title,
      Plot = plot,
      Genre = genre,
      Director = director,
      Premier = premier
    };

    SaveMovie(movie);

    Clear();

    WriteLine("Film Sparad");

    Thread.Sleep(2000);
  }

  private static void SaveMovie(Movie movie)
  {
    using var context = new ApplicationDbContext();

    context.Movie.Add(movie);

    context.SaveChanges();
  }

  private static string GetUserInput(string label)
  {
    Write($"{label}: ");

    return ReadLine() ?? "";
  }
}
