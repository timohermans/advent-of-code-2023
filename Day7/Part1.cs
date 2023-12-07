using System.Diagnostics;
using System.Diagnostics.SymbolStore;
using System.Security.Cryptography;

namespace Day7;

public static class Part1
{
    [DebuggerDisplay("{Rank}: {HandString} - {Bid} -> {HandTypeName} ({HandType})")]
    public class Hand
    {
        public string HandString { get; private set; }
        public List<int> Cards { get; set; } = new();
        public long Bid { get; set; }
        public int? Rank { get; set; }

        public int HandType { get; private set; }
        public string HandTypeName => HandType switch
        {
            15 => "One pair",
            16 => "Two pair",
            17 => "Three of a kind",
            18 => "Full house",
            19 => "Four of a kind",
            20 => "Five of a kind",
            _ => "High card"
        };

        public Hand(string hand)
        {
            var cb = hand.Split(' ');
            HandString = cb[0];
            Cards = cb[0]
                .ToCharArray()
                .Select(c => c switch
                {
                    'T' => 10,
                    'J' => 11,
                    'Q' => 12,
                    'K' => 13,
                    'A' => 14,
                    _ => int.Parse(c.ToString())
                }).ToList();
            Bid = int.Parse(cb[1]);

            var cardPerValue = Cards
                .GroupBy(c => c)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.ToList()) ?? throw new InvalidOperationException("Coudl not be null?");

            if (cardPerValue.Count == 1)
            {
                HandType = 20;
            }
            else if (cardPerValue.Count == 2 && cardPerValue.Any(cpv => cpv.Value.Count == 4))
            {
                HandType = 19;
            }
            else if (cardPerValue.Count == 2 && (cardPerValue.ElementAt(0).Value.Count == 2 || cardPerValue.ElementAt(0).Value.Count == 3))
            {
                HandType = 18;
            }
            else if (cardPerValue.Count == 3 && cardPerValue.Any(cpv => cpv.Value.Count == 3))
            {
                HandType = 17;
            }
            else if (cardPerValue.Values.Where(cpv => cpv.Count == 2).Count() == 2)
            {
                HandType = 16;
            }
            else if (cardPerValue.Values.Where(cpv => cpv.Count == 2).Count() == 1)
            {
                HandType = 15;
            }
            else
            {
                HandType = 14;
            }
        }

    }

    public class HandComparer : IComparer<Hand>
    {
        public int Compare(Hand? h1, Hand? h2)
        {
            if (h1 == null || h2 == null) throw new InvalidOperationException();

            if (h1.HandType < h2.HandType) return -1;
            if (h1.HandType > h2.HandType) return 1;

            for (int i = 0; i < h1.Cards.Count; i++)
            {
                if (h1.Cards[i] == h2.Cards[i]) continue;
                if (h1.Cards[i] < h2.Cards[i]) return -1;
                else return 1;
            }

            Console.WriteLine("Hand is all the same?!!?!?");
            return 0;
        }
    }


    public static void Go()
    {
        var comparer = new HandComparer();
        var hands = File.ReadAllText("input.txt")
            .Split(Environment.NewLine)
            .Select(h => new Hand(h))
                //.OrderBy(h => h.HandValue)
                .OrderBy(h => h, comparer)
            .Select((c, i) =>
            {
                c.Rank = i + 1;
                return c;
            })
            .ToList();

        System.Console.WriteLine(hands.Sum(h => h.Rank * h.Bid));
    }
}