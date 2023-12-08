namespace Day8;

public static class Part1
{
    record Map(string Location, string Left, string Right);
    
    public static void Go()
    {
        var lines = File.ReadAllText("input.txt")
            .Split(Environment.NewLine);

        var instructions = lines.First();

        Map? currentLocation = null;
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

                if (map.Location == "AAA") currentLocation = map;
                return map;
            })
            .ToDictionary(m => m.Location);

        if (currentLocation == null) throw new NullReferenceException(nameof(currentLocation));

        int steps = 0;

        while (currentLocation.Location != "ZZZ")
        {
            foreach (var instruction in instructions)
            {
                if (currentLocation.Location == "ZZZ") break;

                currentLocation = instruction switch
                {
                    'L' => maps[currentLocation.Left],
                    'R' => maps[currentLocation.Right],
                    _ => throw new NotImplementedException()
                };

                steps++;
            }
        }

        Console.WriteLine(steps);
    }
}