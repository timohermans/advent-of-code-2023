namespace Day4;


public static class Part2
{
    public record Card
    {
        public int GameId { get; set; }
        public List<int> Answers { get; set; }
        public List<int> Numbers { get; set; }
        public int Instances { get; set; } = 1;

        public Card(string line)
        {
            var lineSplit = line.Split(':');
            GameId = int.Parse(lineSplit[0].Split(' ', StringSplitOptions.RemoveEmptyEntries)[1]);

            var entireCard = lineSplit[1].Split('|');
            Answers = entireCard[0].Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(n => int.Parse(n)).ToList();
            Numbers = entireCard[1].Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(n => int.Parse(n)).ToList();
        }

        public int DetermineAmountOfMatchingNumbers() {
            return Numbers.Aggregate(0, (winners, number) => {
                if (Answers.Contains(number)) {
                    return winners += 1;
                }
                return winners;
            });
        }

    }

    public static void Go()
    {
        var cards = File.ReadAllText("input.txt")
            .Split(Environment.NewLine)
            .Select(line => new Card(line))
                .ToDictionary(k => k.GameId);
        
        foreach (var cardKvp in cards)
        {
            int winners = cardKvp.Value.DetermineAmountOfMatchingNumbers();

            for (int i = 1; i <= winners; i++)
            {
                try {
                    cards[cardKvp.Key + i].Instances += cardKvp.Value.Instances;
                } catch (IndexOutOfRangeException) {
                    System.Console.WriteLine("Skipping card set, because of index out of bounds");
                }
            }
        }

        var sum = cards.Values.Sum(c => c.Instances);

        System.Console.WriteLine(sum);

    }
}