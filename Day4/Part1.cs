namespace Day4;

public record Card(List<int> Answers, List<int> Numbers);

public static class Part1
{
    public static void Go()
    {
        int sum = File.ReadAllText("input.txt")
            .Split(Environment.NewLine)
            .Select(line => line.Split(':')[1])
            .Select(entireCard => entireCard.Split('|'))
            .Select(cardSplit => new Card(
                cardSplit[0].Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(n => int.Parse(n)).ToList(),
                cardSplit[1].Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(n => int.Parse(n)).ToList()
                ))
            .Sum(card =>
                card.Numbers.Aggregate(0, (points, number) =>
                {
                    if (card.Answers.Contains(number))
                    {
                        return points == 0 ? 1 : points * 2;
                    }
                    return points;
                })
        );

        System.Console.WriteLine(sum);
    }
}