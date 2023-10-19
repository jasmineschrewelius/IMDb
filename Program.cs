using IMDb.Data;
using IMDb.Domain;
using Microsoft.EntityFrameworkCore;
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

      WriteLine("2. Lägg till skådespelare");

      WriteLine("3. Lägg till skådespelare till film");

      WriteLine("4. Sök film");

      var keyPressed = ReadKey(true);

      Clear();

      switch (keyPressed.Key)
      {
        case ConsoleKey.D1:
        case ConsoleKey.NumPad1:

          AddMovieView();

          break;

        case ConsoleKey.D2:
        case ConsoleKey.NumPad2:

          AddActorView();

          break;  

        case ConsoleKey.D3:
        case ConsoleKey.NumPad3:

          AddActorToMovieView();

          break; 

        case ConsoleKey.D4:
        case ConsoleKey.NumPad4:

          SearchMovieView();

          break;  
      }

      Clear();
    }
  }

    private static void SearchMovieView()
    {
      var movieTitle = GetUserInput("Ange titel");

      using var context = new ApplicationDbContext();

      var movie = context.Movie
        // Include() gör att en Join körs (LEFT JOIN) - kommer i detta fallet involvera 3 tabeller
        // i SQL-frågan
        .Include(x => x.Actors)
        .FirstOrDefault(x => x.Title == movieTitle);

      if (movie is not null)
      {
        WriteLine("# -------------------------------------------------------");
        WriteLine($"# {movie.Title}");
        WriteLine("# -------------------------------------------------------");
        
        WriteLine();
        WriteLine(movie.Plot);
        WriteLine();

        WriteLine($"Genre: {movie.Genre}");
        WriteLine($"Regissör: {movie.Director}");
        WriteLine($"År: {movie.Premier.Year}");
        WriteLine();

        WriteLine("Skådespelare");

        foreach (var actor in movie.Actors)
        {
          var deceased = actor.IsDeceased ? "[Avliden]" : "";

          WriteLine($" - {actor.FirstName} {actor.LastName} ({actor.Age} år) {deceased}");
        }

        WaitUntilKeyPressed(ConsoleKey.Escape);
      }
      else
      {
        WriteLine("Film saknas ='(");
        Thread.Sleep(2000);
      }
    }

    private static void WaitUntilKeyPressed(ConsoleKey key)
    {
      while (ReadKey(true).Key != key);
    }

    private static void AddActorToMovieView()
    {
      var actorFullName = GetUserInput("Skådespelare");
      var movieTitle = GetUserInput("Film");

    // ["Sigourney", "Weaver"]  <- "Sigourney Weaver"
      var actorFullNameParts = actorFullName.Split(" ");

      var actorFirstName = actorFullNameParts[0];
      var actorLastName = actorFullNameParts[1];

      using var context = new ApplicationDbContext();

      var actor = context
        .Actor
        .FirstOrDefault(x => x.FirstName == actorFirstName && x.LastName == actorLastName);

      var movie = context
        .Movie
        .FirstOrDefault(x => x.Title == movieTitle);

      movie.Actors.Add(actor);

      context.SaveChanges();

      Clear();
 
      WriteLine("Skådespelare tillagd till film");

      Thread.Sleep(2000);
    }

    private static void AddActorView()
    {
      var firstName = GetUserInput("Förnamn");
      var lastName = GetUserInput("Efternamn");
      var birthDate = DateTime.Parse(GetUserInput("Födelsedatum"));

      WriteLine();

      WriteLine("Är personen avliden? (J)a / (N)ej" );

      ConsoleKeyInfo keyPressed;

      do 
      {
        keyPressed = ReadKey(true);
      } while (!(keyPressed.Key == ConsoleKey.J || keyPressed.Key == ConsoleKey.N));

      DateTime? descesedDate = null;

      if (keyPressed.Key == ConsoleKey.J) {

        descesedDate = DateTime.Parse(GetUserInput("Datum (YYYY-MM-DD)"));
      }

        var actor = new Actor
        {
          FirstName = firstName,
          LastName = lastName,
          BirthDate = birthDate
        };

        SaveActor(actor);

        Clear();

        WriteLine("Skådespelare sparad");

        Thread.Sleep(2000);
    }

    private static void SaveActor(Actor actor)
    {
        using var context = new ApplicationDbContext();

        context.Actor.Add(actor);

        context.SaveChanges();
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
