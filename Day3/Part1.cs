using System.Reflection.PortableExecutable;

namespace Day3;

public static class Part1
{
    public static void Go()
    {
        //         var input = @"467..114..
        // ...*......
        // ..35..633.
        // ......#...
        // 617*......
        // .....+.58.
        // ..592.....
        // ......755.
        // ...$.*....
        // .664.598..";
        var input = File.ReadAllText("input.txt");

        var lines = input.Split(Environment.NewLine);
        long sum = 0;

        for (int lineIndex = 0; lineIndex < lines.Length; lineIndex++)
        {
            string line = lines[lineIndex];
            string? currentNumber = null;
            char? symbolFound = null;

            for (int characterIndex = 0; characterIndex < line.Length; characterIndex++)
            {
                char character = line[characterIndex];

                if (currentNumber == null && char.IsDigit(character))
                {
                    currentNumber = character.ToString();
                }
                else if (char.IsDigit(character))
                {
                    currentNumber += character.ToString();
                }

                if (char.IsDigit(character) && symbolFound == null && currentNumber != null)
                {
                    symbolFound = IsAroundSymbol(characterIndex, lineIndex, lines);
                }

                if (!char.IsDigit(character) || characterIndex == line.Length - 1)
                {
                    if (currentNumber != null && symbolFound != null)
                    {
                        System.Console.WriteLine($"[{lineIndex + 1},{characterIndex + 1}]: {currentNumber} ({symbolFound})");
                        sum += Convert.ToInt32(currentNumber);
                    }
                    currentNumber = null;
                    symbolFound = null;
                }
            }
        }

        System.Console.WriteLine(sum);
    }

    public static bool IsSymbol(char character)
    {
        return character != '.' && !char.IsDigit(character);
    }

    public static char? IsAroundSymbol(int characterIndex, int lineIndex, IEnumerable<string> lines)
    {
        var isTop = TryIsAt(characterIndex, lineIndex - 1, lines);
        var isTopRight = TryIsAt(characterIndex + 1, lineIndex - 1, lines);
        var isRight = TryIsAt(characterIndex + 1, lineIndex, lines);
        var isBottomRight = TryIsAt(characterIndex + 1, lineIndex + 1, lines);
        var isBottom = TryIsAt(characterIndex, lineIndex + 1, lines);
        var isBottomLeft = TryIsAt(characterIndex - 1, lineIndex + 1, lines);
        var isLeft = TryIsAt(characterIndex - 1, lineIndex, lines);
        var isTopLeft = TryIsAt(characterIndex - 1, lineIndex - 1, lines);

        return isTop ?? isTopRight ?? isRight ?? isBottomRight ?? isBottom ?? isBottomLeft ?? isLeft ?? isTopLeft;
    }

    public static char? TryIsAt(int characterIndex, int lineIndex, IEnumerable<string> lines)
    {
        try
        {
            if (IsSymbol(lines.ElementAt(lineIndex).ElementAt(characterIndex)))
            {
                return lines.ElementAt(lineIndex).ElementAt(characterIndex);
            }

            return null;
        }
        catch (ArgumentOutOfRangeException)
        {
            return null;
        }
    }

}