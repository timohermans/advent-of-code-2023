namespace Day1;

public static class Part1
{
    public static void Go()
    {
        var input = File.ReadAllText("input.txt");

        var lines = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        var digits = new int[lines.Length];

        foreach (var (line, index) in lines.Select((l, i) => (l, i)))
        {
            char firstDigit = line.First(c => int.TryParse(c.ToString(), out int _));
            char lastDigit = line.Reverse().First(c => int.TryParse(c.ToString(), out int _));
            char[] digit = [firstDigit, lastDigit];

            digits[index] = Convert.ToInt32(new string(digit));
        }

        Console.WriteLine(digits.Sum());
    }
}