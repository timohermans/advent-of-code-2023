using System.Diagnostics;
using System.Diagnostics.SymbolStore;
using System.Security.Cryptography;

namespace Day7;

public static class Part2
{
    [DebuggerDisplay("{Rank}: {HandString} - {Bid} -> {HandValueName} ({HandType})")]
    public class Hand
    {
        public string HandString { get; private set; }
        public List<int> Cards { get; set; } = new();
        public long Bid { get; set; }
        public int? Rank { get; set; }

        public int HandType { get; private set; }
        public string HandValueName => HandType switch
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
                    'J' => -1,
                    'Q' => 12,
                    'K' => 13,
                    'A' => 14,
                    _ => int.Parse(c.ToString())
                }).ToList();
            Bid = int.Parse(cb[1]);

            var cardPerValue = Cards
                .GroupBy(c => c)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.ToList()) ?? throw new InvalidOperationException("Coudl not be null?");

            int joker = -1;
            if (!cardPerValue.ContainsKey(joker)) cardPerValue.Add(joker, new List<int>());

            // 22222
            // JJJJ2, JJJ33, etc.
            // JJJJJ
            if (
                (cardPerValue.Count == 2 && cardPerValue[joker].Count == 0)
                || (cardPerValue.Count == 2 && cardPerValue[joker].Count > 0)
                || (cardPerValue.Count == 1))
            {
                HandType = 20;
            }

            // four of a kinds:
            // 23333 <-- 1
            // 2333J <-- 2
            // 233JJ <-- 3
            // 23JJJ <-- 4
            // x 2JJJJ 
            else if (
                (cardPerValue.Count == 3 && cardPerValue.Any(cpv => cpv.Value.Count == 4) && cardPerValue[joker].Count == 0) //<-- 1
                || cardPerValue.Count == 3 && cardPerValue[joker].Count == 1 && cardPerValue.Any(cpv => cpv.Value.Count == 3) //<-- 2
                || cardPerValue.Count == 3 && cardPerValue[joker].Count == 2 && cardPerValue.Any(cpv => cpv.Value.Count == 2) //<-- 3
                || cardPerValue.Count == 3 && cardPerValue[joker].Count == 2 && cardPerValue.Any(cpv => cpv.Value.Count == 2) //<-- 3
                || cardPerValue.Count == 3 && cardPerValue[joker].Count == 3 && cardPerValue.Any(cpv => cpv.Value.Count == 1) //<-- 3
                )
            {
                HandType = 19;
            }
            // full house:
            // 22333 <-- 1
            // 2233J <-- 2
            else if (
                (cardPerValue.Count == 3 && cardPerValue.Any(cpv => cpv.Value.Count == 3) && cardPerValue.Any(cpv => cpv.Value.Count == 2) && cardPerValue[joker].Count == 0)
                || (cardPerValue.Count == 3 && cardPerValue.Where(cpv => cpv.Value.Count == 2).Count() == 2 && cardPerValue[joker].Count == 1)
             )
            {
                HandType = 18;
            }

            // Three of a kind:
            // 11198
            // J1198
            // JJ198
            else if (
                (cardPerValue.Count == 4 && cardPerValue.Any(cpv => cpv.Value.Count == 3) && cardPerValue[joker].Count == 0)
                || (cardPerValue.Count == 4 && cardPerValue.Any(cpv => cpv.Value.Count == 2) && cardPerValue[joker].Count == 1)
                || (cardPerValue.Count == 4 && cardPerValue[joker].Count == 2)
                    )
            {
                HandType = 17;
            }

            // Two pair:
            // 11223
            // x 1123J
            else if (cardPerValue.Count == 4 && cardPerValue[joker].Count == 0 && cardPerValue.Where(cpv => cpv.Value.Count == 2).Count() == 2)
            {
                HandType = 16;
            }

            // One pair:
            // J1234
            else if (
                (cardPerValue.Values.Where(cpv => cpv.Count == 2).Count() == 1 && cardPerValue[joker].Count == 0)
                || cardPerValue.Count == 5 && cardPerValue[joker].Count == 1
                )
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