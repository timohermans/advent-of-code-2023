using System.Text.RegularExpressions;

namespace Day2;

public static class Part1
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
        var sumIds = 0;

        foreach (var line in lines)
        {
            int id = Convert.ToInt32(new Regex("Game (\\d+)").Match(line).Groups[1].Value);
            bool isRedBelow = IsUnderThreshold(line, "red", 12);
            bool isGreenBelow = IsUnderThreshold(line, "green", 13);
            bool isBlueBelow = IsUnderThreshold(line, "blue", 14);

            if (isRedBelow && isGreenBelow && isBlueBelow)
            {
                sumIds += id;
            }
        }

        System.Console.WriteLine(sumIds);
    }

    private static bool IsUnderThreshold(string line, string color, int threshold)
    {
        var pattern = new Regex($"(\\d+) {color}");
        var matches = pattern.Matches(line);

        if (matches == null) return true;

        foreach (Match match in matches)
        {
            for (int i = 1; i < match.Groups.Count; i++)
            {
                if (Convert.ToInt32(match.Groups[i].Value) > threshold)
                {
                    return false;
                }
            }
        }

        return true;
    }
}
