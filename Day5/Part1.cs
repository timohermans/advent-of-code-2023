namespace Day5;

public static class Part1
{


    public static void Go()
    {
        var input = File.OpenText("input.txt");

        var seedsInput = input.ReadLine() ?? throw new InvalidOperationException("Somehow unable to read seeds");
        var seeds = seedsInput.Split(':')[1].Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(s => long.Parse(s)).ToList();

        System.Console.WriteLine("seeds found: " + string.Join(", ", seeds));
        input.ReadLine(); // whiteline

        long[] finalLocations = seeds.ToArray();

        while (!input.EndOfStream)
        {
            string mapTitle = input.ReadLine() ?? throw new InvalidOperationException("the current map instructions is empty? should be the title");
            long?[] nextSoilLocations = new long?[seeds.Count];
            System.Console.WriteLine($"currently doing phase {mapTitle.Split(':')[0]}");
            System.Console.WriteLine($"Initial seeds for this phase: {string.Join(", ", finalLocations)}");

            string? instructionsLine;
            while ((instructionsLine = input.ReadLine()) != "" && instructionsLine != null)
            {
                if (instructionsLine == null)
                {
                    System.Console.WriteLine("Insturctions are null?! breaking");
                    break;
                }

                List<long> instructions = instructionsLine.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(i => long.Parse(i.Trim())).ToList();
                long sourceRangeStart = instructions[1];
                long destinationRangeStart = instructions[0];
                long rangeLength = instructions[2];

                for (int i = 0; i < finalLocations.Length; i++)
                {
                    long currentLocation = finalLocations[i];

                    if (currentLocation >= sourceRangeStart && currentLocation <= (sourceRangeStart + rangeLength - 1))
                    {
                        nextSoilLocations[i] = destinationRangeStart + (currentLocation - sourceRangeStart);
                        System.Console.WriteLine($"[{i}] Seed {finalLocations[i]} > {nextSoilLocations[i]} ({instructionsLine})");
                    }
                }
            }

            for (int i = 0; i < nextSoilLocations.Length; i++)
            {
                if (nextSoilLocations[i] != null) continue;
                System.Console.WriteLine($"[{i}] Seed {finalLocations[i]} stayed the same");
                nextSoilLocations[i] = finalLocations[i];
            }

            finalLocations = nextSoilLocations.Cast<long>().ToArray();
        }

        System.Console.WriteLine(finalLocations.Min());



    }
}