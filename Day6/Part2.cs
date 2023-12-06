using System.Diagnostics;

namespace Day6;

public class Part2
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

        long times = long.Parse(input[0].Replace(" ", ""));
        long distances = long.Parse(input[1].Replace(" ", ""));

        List<long> amounts = new List<long>();
        long amountDistanceReached = 0;
        for (long pressDuration = 0; pressDuration < times; pressDuration++)
        {
            var distanceReached = pressDuration * (times - pressDuration);

            if (distanceReached > distances)
            {
                amountDistanceReached++;
            }
        }

        System.Console.WriteLine(amountDistanceReached);
    }

}