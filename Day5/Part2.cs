using System.Diagnostics;

namespace Day5;

public static class Part2
{
    record MapRange(long DestinationLocation, long SourceLocation, long Range);

    [DebuggerDisplay("{Name}")]
    class Map
    {
        public string Name { get; set; }
        public List<MapRange> Ranges { get; set; } = new();
    }

    class Almanac
    {
        public List<long> Seeds { get; set; } = new();
        public List<Map> Maps { get; set; } = new();
    }


    public static void Go()
    {
        var almanac = new Almanac();
        long[] seeds;
        using (var input = File.OpenText("input.txt"))
        {
            var seedsInput = input.ReadLine() ?? throw new InvalidOperationException("Somehow unable to read seeds");
            seeds = seedsInput.Split(':')[1].Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(s => long.Parse(s)).ToArray();

            System.Console.WriteLine("seeds found: " + string.Join(", ", seeds));
            input.ReadLine(); // whiteline

            // almanac.Seeds = seeds;

            Console.WriteLine("going to do magic");


            while (!input.EndOfStream)
            {
                Map map = new Map();
                map.Name = input.ReadLine() ?? throw new InvalidOperationException("the current map instructions is empty? should be the title");

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
                    map.Ranges.Add(new MapRange(destinationRangeStart, sourceRangeStart, rangeLength));
                }

                almanac.Maps.Add(map);
            }
        }

        //
        object lowestLock = new object();
        long lowest = long.MaxValue;

        Parallel.ForEach(seeds.Chunk(2), seedRange =>
        {
            long currentLowest = long.MaxValue;
            for (long seed = seedRange[0]; seed < seedRange[0] + seedRange[1]; seed++)
            {
                long location = seed;

                // System.(location);

                foreach (var map in almanac.Maps)
                {
                    foreach (var range in map.Ranges)
                    {
                        if (location >= range.SourceLocation && location <= (range.SourceLocation + range.Range - 1))
                        {
                            location = range.DestinationLocation + (location - range.SourceLocation);
                            break;
                        }
                    }

                    // System.Console.Write($" > {location}");
                }

                if (location < currentLowest)
                {
                    currentLowest = location;
                }
            }

            lock (lowestLock)
            {
                if (currentLowest < lowest)
                {
                    lowest = currentLowest;
                }
            }
            System.Console.WriteLine("done with range: " + currentLowest);
        });

        System.Console.WriteLine(lowest);
    }
}