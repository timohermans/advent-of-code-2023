using System.Text.RegularExpressions;

namespace Day2;

public static class Part2
{
    public static void Go()
    {
//         string input = @"Game 1: 3 blue, 4 red; 1 red, 2 green, 6 blue; 2 green
// Game 2: 1 blue, 2 green; 3 green, 4 blue, 1 red; 1 green, 1 blue
// Game 3: 8 green, 6 blue, 20 red; 5 blue, 4 red, 13 green; 5 green, 1 red
// Game 4: 1 green, 3 red, 6 blue; 3 green, 6 red; 3 green, 15 blue, 14 red
// Game 5: 6 red, 1 blue, 3 green; 2 blue, 1 red, 2 green";
        string input = File.ReadAllText("part1.txt");

        var lines = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        long sumPowers = 0;

        foreach (var line in lines)
        {
            int reds = GetMostOfSingle(line, "red");
            int greens = GetMostOfSingle(line, "green");
            int blues = GetMostOfSingle(line, "blue");

            int power = reds * greens * blues;
            sumPowers += power;
        }

        System.Console.WriteLine(sumPowers);
    }

    private static int GetMostOfSingle(string line, string color)
    {
        var pattern = new Regex($"(\\d+) {color}");
        var matches = pattern.Matches(line);

        if (matches == null) return 0;

        int fewest = 0;
        foreach (Match match in matches)
        {
            int amount = Convert.ToInt32(match.Groups[1].Value);
            if (amount > fewest) fewest = amount;
        }

        return fewest;
    }
}
