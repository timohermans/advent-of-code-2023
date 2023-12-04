using System.Text.RegularExpressions;

namespace Day1;

public static class Part2
{
    public static void Go()
    {
        string input = File.ReadAllText("part2.txt");
        IEnumerable<string> lines = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

        string[] translations = ["zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine"];
        string pattern = $"(\\d|{string.Join('|', translations)})";
        Regex regex = new Regex(pattern);
        List<int> digits = new List<int>();

        foreach (var line in lines)
        {
            List<Match> matches = new List<Match>();
            for (int i = 0; i < line.Length; i++)
            {
                string lineToMatch = line.Substring(i);
                var match = regex.Match(lineToMatch);
                if (match.Value != "") matches.Add(match);
            }
            var firstMatch = matches.First();
            var lastMatch = matches.Last();

            string firstDigit = firstMatch.Length == 1 ? firstMatch.Value : Array.IndexOf(translations, firstMatch.Value).ToString();
            string lastDigit = lastMatch.Length == 1 ? lastMatch.Value : Array.IndexOf(translations, lastMatch.Value).ToString();
            digits.Add(Convert.ToInt32(firstDigit + lastDigit));
        }

        Console.WriteLine($"Part 2: {digits.Sum()}");

    }
}