using System.ComponentModel;

namespace Day8;

public static class Part2
{
    record Map(string Location, string Left, string Right);

    public static void Go()
    {
        var lines = File.ReadAllText("input.txt")
            .Split(Environment.NewLine);

        var instructions = lines.First();

        var maps = lines
            .Skip(2)
            .Select(l =>
            {
                var locationAndNext = l
                    .Split(" = ", StringSplitOptions.RemoveEmptyEntries);
                var nextLocations = locationAndNext[1].Split(", ");
                var left = string.Join("", nextLocations[0].Skip(1).Take(3)) ?? throw new InvalidOperationException();
                var right = string.Join("", nextLocations[1].Take(3)) ?? throw new InvalidOperationException();
                var map = new Map(locationAndNext[0], left, right);
                return map;
            })
            .ToDictionary(m => m.Location);

        List<Map> originalLocations = maps.Where(m => m.Key.EndsWith('A')).Select(m => m.Value).ToList();
        List<Map> currentLocations = maps.Where(m => m.Key.EndsWith('A')).Select(m => m.Value).ToList();


        List<int> stepsList = new List<int>();

        for (int i = 0; i < currentLocations.Count; i++)
        {
            Map currentLocation = currentLocations[i];
            int steps = 0;

            while (!currentLocation.Location.EndsWith('Z'))
            {
                foreach (var instruction in instructions)
                {
                    if (currentLocation.Location.EndsWith('Z')) break;

                    currentLocation = instruction switch
                    {
                        'L' => maps[currentLocation.Left],
                        'R' => maps[currentLocation.Right],
                        _ => throw new NotImplementedException()
                    };

                    steps++;
                }
            }

            stepsList.Add(steps);
        }
    }
}