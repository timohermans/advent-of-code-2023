using System.Reflection.PortableExecutable;

namespace Day3;

public static class Part2
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

            for (int characterIndex = 0; characterIndex < line.Length; characterIndex++)
            {
                var character = line[characterIndex];
                if (!IsGear(character)) continue;
                var numbers = new List<int>();

                for (int checkOffset = -1; checkOffset < 2; checkOffset++)
                {
                    try
                    {
                        var previousLine = lines.ElementAt(lineIndex + checkOffset);
                        var leftMostIndex = characterIndex - 1;
                        var maxIndex = characterIndex + 1;

                        while (leftMostIndex != 0 && char.IsDigit(previousLine.ElementAt(leftMostIndex)))
                        {
                            leftMostIndex--;
                        }

                        var currentIndex = leftMostIndex;
                        string currentNumber = "";

                        do
                        {
                            char currentChar = previousLine.ElementAt(currentIndex);

                            if (char.IsDigit(currentChar))
                            {
                                currentNumber += currentChar;
                            }

                            if (currentNumber != "" && ((currentIndex + 1) > (line.Length - 1) || !char.IsDigit(previousLine.ElementAt(currentIndex + 1))))
                            {
                                numbers.Add(Convert.ToInt32(currentNumber));
                                currentNumber = "";
                            }

                            currentIndex++;
                        } while (currentIndex <= maxIndex || currentNumber != "");
                    }
                    catch (ArgumentException)
                    {
                        // TODO: never be stupid enough to swallow exceptions again
                        System.Console.WriteLine($"Checking out of bounds [{lineIndex + checkOffset},{characterIndex}]");
                    }
                }

                System.Console.WriteLine($"[{lineIndex + 1},{characterIndex + 1}]:{string.Join(" * ", numbers)}");
                if (numbers.Count == 2)
                {
                    sum += numbers.Aggregate((a, b) => a * b);
                }
            }
        }

        System.Console.WriteLine(sum);
    }

    public static bool IsGear(char character)
    {
        return character == '*';
    }

    public static char? IsAroundSymbol(int characterIndex, int lineIndex, IEnumerable<string> lines)
    {
        var isTop = TryIsNumberAt(characterIndex, lineIndex - 1, lines);
        var isTopRight = TryIsNumberAt(characterIndex + 1, lineIndex - 1, lines);
        var isRight = TryIsNumberAt(characterIndex + 1, lineIndex, lines);
        var isBottomRight = TryIsNumberAt(characterIndex + 1, lineIndex + 1, lines);
        var isBottom = TryIsNumberAt(characterIndex, lineIndex + 1, lines);
        var isBottomLeft = TryIsNumberAt(characterIndex - 1, lineIndex + 1, lines);
        var isLeft = TryIsNumberAt(characterIndex - 1, lineIndex, lines);
        var isTopLeft = TryIsNumberAt(characterIndex - 1, lineIndex - 1, lines);

        return isTop ?? isTopRight ?? isRight ?? isBottomRight ?? isBottom ?? isBottomLeft ?? isLeft ?? isTopLeft;
    }

    public static char? TryIsNumberAt(int characterIndex, int lineIndex, IEnumerable<string> lines)
    {
        try
        {
            if (char.IsDigit(lines.ElementAt(lineIndex).ElementAt(characterIndex)))
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