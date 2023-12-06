using System.Diagnostics;

namespace Day6;

public class Part1
{
    [DebuggerDisplay("{Time}ms {Distance}mm")]
    public class Race
    {
        public long Time { get; set; }
        public long Distance { get; set; }
    }

    public static void Go()
    {
        var input = File.ReadAllText("input.txt").Split(Environment.NewLine).Select(s => s.Split(':').Last()).ToList();

        var times = input[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();
        var distances = input[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();
        var races = new List<Race>();

        for (int i = 0; i < times.Count; i++)
        {
            races.Add(new Race {
                Distance = long.Parse(distances[i]),
                Time = long.Parse(times[i])
            });
        }

        List<long> amounts = new List<long>();

        foreach (var race in races)
        {
            long amountDistanceReached = 0;
            for (long pressDuration = 0; pressDuration < race.Time; pressDuration++)
            {
                var distanceReached = pressDuration * (race.Time - pressDuration);

                if (distanceReached > race.Distance) {
                    amountDistanceReached++;
                }
            }

            amounts.Add(amountDistanceReached);
        }

        long result = amounts.Aggregate((a, b) => a * b);

        System.Console.WriteLine(result);

    }

}