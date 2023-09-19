using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

public class Videogame : IComparable
{
    public string Name { get; set; }
    public string Platform { get; set; }
    public DateTime Year { get; set; }
    public string Genre { get; set; }
    public string Publisher { get; set; }
    public float NaSales { get; set; }
    public float EuSales { get; set; }
    public float JpSales { get; set; }
    public float OtherSales { get; set; }
    public float GlobalSales { get; set; }

    public Videogame(string csvInput)
    {
        var elements = csvInput.Split(',');
        Name = elements[0];
        Platform = elements[1];
        Year = DateTime.Parse("01/01/" + elements[2]);
        Genre = elements[3];
        Publisher = elements[4];
        NaSales = float.Parse(elements[5]);
        EuSales = float.Parse(elements[6]);
        JpSales = float.Parse(elements[7]);
        OtherSales = float.Parse(elements[8]);
        GlobalSales = float.Parse(elements[9]);
    }

    public override string ToString()
    {
        return $"{nameof(Name)}: {Name}, " +
               $"{nameof(Platform)}: {Platform}, " +
               $"{nameof(Year)}: {Year}, " +
               $"{nameof(Genre)}: {Genre}, " +
               $"{nameof(Publisher)}: {Publisher}, " +
               $"{nameof(NaSales)}: {NaSales.ToString("F2", CultureInfo.InvariantCulture)}, " +
               $"{nameof(EuSales)}: {EuSales.ToString("F2", CultureInfo.InvariantCulture)}, " +
               $"{nameof(JpSales)}: {JpSales.ToString("F2", CultureInfo.InvariantCulture)}, " +
               $"{nameof(OtherSales)}: {OtherSales.ToString("F2", CultureInfo.InvariantCulture)}, " +
               $"{nameof(GlobalSales)}: {GlobalSales.ToString("F2", CultureInfo.InvariantCulture)}";
    }

    public int CompareTo(object obj)
    {
        if (obj is Videogame vid)
        {
            return string.Compare(this.Name, vid.Name, StringComparison.InvariantCultureIgnoreCase);
        }
        return -1;
    }
}

public class GameLibrary
{
    private Dictionary<string, List<Videogame>> gamesByPlatform;

    public GameLibrary(List<Videogame> games)
    {
        gamesByPlatform = games
            .GroupBy(game => game.Platform)
            .ToDictionary(group => group.Key, group => group.ToList());
    }

    public List<Videogame> TopGamesForPlatform(string platform)
    {
        if (gamesByPlatform.TryGetValue(platform, out var games))
        {
            return games.OrderByDescending(game => game.GlobalSales).Take(5).ToList();
        }
        return new List<Videogame>();
    }

    public void DisplayTopGamesForAllPlatforms()
    {
        foreach (var platform in gamesByPlatform.Keys)
        {
            Console.WriteLine($"Top 5 games for {platform}:");

            var topGames = TopGamesForPlatform(platform);
            foreach (var game in topGames)
            {
                Console.WriteLine(game);
            }

            Console.WriteLine();
        }
    }
}

public static class Program
{
    public static void Main()
    {
        var lines = File.ReadLines("videogames.csv");
        List<Videogame> gameList = lines.Skip(1).Select(line => new Videogame(line)).ToList();

        GameLibrary library = new GameLibrary(gameList);
        library.DisplayTopGamesForAllPlatforms();

        Console.WriteLine("Press Enter to exit");
        Console.ReadLine();
    }
}