using DataBasesHW7.Models;
using AppContext = DataBasesHW7.Database.AppContext;

namespace DataBasesHW7.Management;

public static class Database
{
    private const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    
    private static readonly Random Random = new();

    private static readonly List<string> Time = new() { "seconds", "meters" };
    
    private static readonly List<string> Medals = new() { "GOLD", "SILVER", "BRONZE" };

    public static void CreateDatabase(AppContext db)
    {
        var country = new Country
        {
            Name = "Australia",
            Id = "AUS",
            AreaSqkm = 7682300,
            Population = 21050000,
            Olympiads = new List<Olympiad>()
        };

        var olympiad = new Olympiad
        {
            Id = "SYD2000",
            CountryId = country.Id,
            City = "Sydney",
            Year = 2000,
            StartDate = new DateTime(2000, 09, 15),
            EndDate = new DateTime(2000, 10, 01),
            Country = country,
            Events = new List<Event>()
        };
        country.Olympiads.Add(olympiad);

        var olympiadEvent = new Event
        {
            Id = "E1",
            Name = "10000m Men",
            EventType = "ATH",
            OlympiadId = olympiad.Id,
            IsTeamEvent = 0,
            NumSportsmenInTeam = -1,
            ResultNotedIn = "seconds"
        };
        olympiad.Events.Add(olympiadEvent);
        olympiadEvent.Olympiad = olympiad;
        olympiadEvent.Results = new List<Result>();

        var result = new Result
        {
            Id = olympiadEvent.Id,
            Medal = "GOLD",
            SportsmanResult = 9.87
        };
        olympiadEvent.Results.Add(result);
        result.Event = olympiadEvent;
        
        var sportsman = new Sportsman
        {
            Results = new List<Result>(),
            CountryId = country.Id,
            Id = "000000000",
            Name = "Ivan Ivanov",
            BirthDate = new DateTime(1978, 04, 21)
        };
        sportsman.Results.Add(result);
        result.Id = result.EventId + sportsman.Id;
        result.Sportsman = sportsman;
        result.SportsmanId = sportsman.Id;

        db.Sportsmen?.Add(sportsman);
        db.Countries?.Add(country);
        db.Olympiads?.Add(olympiad);
        db.Events?.Add(olympiadEvent);
        db.Results?.Add(result);
        db.SaveChanges();
    }

    public static void ShowAllTables(AppContext db)
    {
        if (db.Events is null)
        {
            throw new NullReferenceException("Events is null!");
        }
        
        if (db.Sportsmen is null)
        {
            throw new NullReferenceException("Sportsmen is null!");
        }
        
        if (db.Results is null)
        {
            throw new NullReferenceException("Results is null!");
        }
        
        if (db.Olympiads is null)
        {
            throw new NullReferenceException("Olympiads is null!");
        }
        
        if (db.Countries is null)
        {
            throw new NullReferenceException("Countries is null!");
        }
        
        var sportsmen = db.Sportsmen.ToList();
        Console.WriteLine("Sportsmen:" + Environment.NewLine + "|Id|Name|Id|BirthDate|");
        foreach (var s in sportsmen)
        {
            Console.WriteLine($"{s.Id}. {s.Name} {s.Id} {s.BirthDate.ToString($"dd/MM/yyyy")}");
        }

        var olympiads = db.Olympiads.ToList();
        Console.WriteLine("Olympiads:\n|Id|Id|City|Year|StartDate|EndDate|");
        foreach (var o in olympiads)
        {
            Console.WriteLine($"{o.Id}. {o.Id} {o.City} {o.Year} {o.StartDate.ToString($"dd/MM/yyyy")} {o.EndDate.ToString($"dd/MM/yyyy")}");
        }

        var countries = db.Countries.ToList();
        Console.WriteLine("Countries:\n|Id|Name|AreaSqkm|Population|");
        foreach (var c in countries)
        {
            Console.WriteLine($"{c.Id}. {c.Name} {c.AreaSqkm} {c.Population} ");
        }

        var events = db.Events.ToList();
        Console.WriteLine("Events:\n|Id|Name|EventType|Id|IsTeamEvent|NumSportsmenInTeam|ResultNotedIn|");
        foreach (var e in events)
        {
            Console.WriteLine($"{e.Id} {e.Name} {e.EventType} {e.Id} {e.IsTeamEvent} {e.NumSportsmenInTeam} {e.ResultNotedIn}");
        }

        var results = db.Results.ToList();
        Console.WriteLine("Results:\n|Id|Id|Medal|SportsmanResult|");
        foreach (var r in results)
        {
            Console.WriteLine($"{r.Id} {r.Id} {r.Medal} {r.SportsmanResult}");
        }
    }
    
    public static void AddSportsman(AppContext db)
    {
        if (db.Countries is null)
        {
            throw new NullReferenceException("Countries is null!");
        }
        
        if (db.Sportsmen is null)
        {
            throw new NullReferenceException("Sportsmen is null!");
        }
        
        var toSkip = Random.Next(1, db.Countries.Count());
        var sportsman = new Sportsman
        {
            Results = new List<Result>(),
            CountryId = db.Countries.Skip(toSkip).Take(1).First().Id,
            Id = GetName(),
            Name = $"{GetName()} {GetName()}",
            BirthDate = GetRandomDate(DateTime.Today)
        };

        db.Sportsmen.Add(sportsman);
        db.SaveChanges();
    }
    
    public static void AddCountry(AppContext db)
    {
        if (db.Countries is null)
        {
            throw new NullReferenceException("Countries is null!");
        }
        
        var country = new Country
        {
            Name = GetName(),
            AreaSqkm = Random.Next(10, 100000),
            Population = Random.Next(10000, 100000),
            Olympiads = new List<Olympiad>()
        };
        country.Id = GetRandomString(3, country.Name);
        db.Countries.Add(country);
        db.SaveChanges();
    }

    public static void AddOlympiad(AppContext db)
    {
        if (db.Countries is null)
        {
            throw new NullReferenceException("Countries is null!");
        }
        
        if (db.Olympiads is null)
        {
            throw new NullReferenceException("Olympiads is null!");
        }
        
        var olympiad = new Olympiad();
        var year = Random.Next(1900, 2021);
        olympiad.Id = GetRandomString(3, Alphabet) + year;
        olympiad.City = GetName();
        olympiad.Year = year;
        olympiad.StartDate = GetRandomDate(DateTime.Today);
        olympiad.EndDate = GetRandomDate(olympiad.StartDate);
        if (db.Countries.Any())
        {
            var toSkip = Random.Next(db.Countries.Count());
            var country = db.Countries.Skip(toSkip).Take(1).First();
            olympiad.Country = country;
            olympiad.Id = country.Id;
            country.Olympiads ??= new List<Olympiad>();
            country.Olympiads.Add(olympiad);
        }

        db.Olympiads.Add(olympiad);
        db.SaveChangesAsync();
    }
    
    public static void AddEvent(AppContext db)
    {
        if (db.Olympiads is null)
        {
            throw new NullReferenceException("Olympiads is null!");
        }
        
        if (db.Events is null)
        {
            throw new NullReferenceException("Events is null!");
        }
        
        var toSkip = Random.Next(db.Olympiads.Count());
        var olympiad = db.Olympiads.Skip(toSkip).Take(1).First();
        var olympiadEvent = new Event
        {
            Id = "E" + (db.Events.Count() + 1),
            Name = GetName(),
            EventType = GetRandomString(3, Alphabet),
            OlympiadId = olympiad.Id,
            IsTeamEvent = Random.Next(1),
            ResultNotedIn = Time[Random.Next(2)],
            Olympiad = olympiad,
            Results = new List<Result>()
        };
        olympiadEvent.NumSportsmenInTeam = olympiadEvent.IsTeamEvent > 0 ? Random.Next(2, 20) : -1;
        olympiad.Events.Add(olympiadEvent);

        db.Events.Add(olympiadEvent);
        db.SaveChanges();
    }
    
    public static void AddResult(AppContext db)
    {
        if (db.Events is null)
        {
            throw new NullReferenceException("Events is null!");
        }
        
        if (db.Sportsmen is null)
        {
            throw new NullReferenceException("Sportsmen is null!");
        }
        
        if (db.Results is null)
        {
            throw new NullReferenceException("Results is null!");
        }
        
        var toSkipEvent = Random.Next(db.Events.Count());
        var toSkipSportsman = Random.Next(db.Sportsmen.Count());
        var olympiadEvent = db.Events.Skip(toSkipEvent).Take(1).First();
        var sportsman = db.Sportsmen.Skip(toSkipSportsman).Take(1).First();
        var result = new Result
        {
            Event = olympiadEvent,
            EventId = olympiadEvent.Id,
            Sportsman = sportsman,
            SportsmanId = sportsman.Id,
            Medal = Medals[Random.Next(3)],
            SportsmanResult = Random.NextDouble() * Random.Next(40),
            Id = "R" + (db.Results.Count() + 1)
        };
        if (olympiadEvent.Results?.Count == 0)
        {
            olympiadEvent.Results = new List<Result>();
        }

        olympiadEvent.Results?.Add(result);
        sportsman.Results?.Add(result);

        db.Results.Add(result);
        db.SaveChanges();
    }

    private static string GetRandomString(int length, string source)
    {
        return new string(Enumerable.Repeat(source, length)
            .Select(s => s[Random.Next(s.Length)]).ToArray());
    }
    
    private static string GetName()
    {
        var nameLength = Random.Next(5, 10);
        var name = GetRandomString(1, Alphabet).ToUpper();
        name += GetRandomString(nameLength, Alphabet);
        return name;
    }

    private static DateTime GetRandomDate(DateTime date)
    {
        var start = new DateTime(1900, 1, 1);
        var range = (date - start).Days;
        return start.AddDays(Random.Next(range));
    }
}