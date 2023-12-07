using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AdventOfCode;
using AdventOfCode.Utils;

namespace AoC2023;

public class Day7 : IAocDay
{
    public async Task<object> Part1()
    {
        int ans = 0;
        var input = await IO.GetInput(2023, 7);
        
        var cards = input.Select(line => line.Split(' ', StringSplitOptions.RemoveEmptyEntries)[0]).Select(l => l.ToList());
        var bids = input.Select(line => line.Split(' ', StringSplitOptions.RemoveEmptyEntries)[1]).Select(int.Parse);

        var hands = cards.Zip(bids).Select(i => new HandPart1(i.First, i.Second)).ToList();
        var ordered = hands.OrderBy(x => x).ToList();
        
        ans = ordered.Select((x, rank) => (rank + 1) * x.Bid).Sum();
        return ans;
    }

    public async Task<object> Part2()
    {
        int ans = 0;
        var input = await IO.GetInput(2023, 7);

        var cards = input.Select(line => line.Split(' ', StringSplitOptions.RemoveEmptyEntries)[0]).Select(l => l.ToList());
        var bids = input.Select(line => line.Split(' ', StringSplitOptions.RemoveEmptyEntries)[1]).Select(int.Parse);

        var hands = cards.Zip(bids).Select(i => new HandPart2(i.First, i.Second)).ToList();
        var ordered = hands.OrderBy(x => x).ToList();

        ans = ordered.Select((x, rank) => (rank + 1) * x.Bid).Sum();
        return ans;
    }
}

[DebuggerDisplay("{AsString}")]
public class HandPart1 : IComparable<HandPart1>
{
    public int Bid { get; }
    public readonly List<char> cards;
    public string AsString => string.Join("", cards);

    public override string ToString()
    {
        return AsString;
    }

    public Dictionary<char, int> cardToPoints = new()
    {
        { 'A', 14 },
        { 'K', 13 },
        { 'Q', 12 },
        { 'J', 11 },
        { 'T', 10 },
        { '9', 9 },
        { '8', 8 },
        { '7', 7 },
        { '6', 6 },
        { '5', 5 },
        { '4', 4 },
        { '3', 3 },
        { '2', 2 },
    };

    public HandPart1(List<char> cards, int bid)
    {
        Bid = bid;
        this.cards = cards;
    }

    public int GetStrength()
    {
        if (cards.Distinct().Count() == 1)
        {
            return 7;
        }

        if (cards.Any(c => cards.Count(x => x == c) == 4))
        {
            return 6;
        }

        if (cards.Any(c => cards.Count(x => x == c) == 3) && cards.Any(c => cards.Count(x => x == c) == 2))
        {
            return 5;
        }

        if (cards.Any(c => cards.Count(x => x == c) == 3))
        {
            return 4;
        }

        var pairs = cards.Where(c => cards.Count(x => x == c) == 2).Distinct().ToList();
        if (pairs.Count == 2)
        {
            return 3;
        }

        if (pairs.Count == 1)
        {
            return 2;
        }

        return 1;
    }


    public int CompareTo(HandPart1 other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (ReferenceEquals(null, other)) return 1;

        var strength = GetStrength();
        var otherStrength = other.GetStrength();

        if (strength > otherStrength)
        {
            return 1;
        }

        if (strength < otherStrength)
        {
            return -1;
        }

        for (int i = 0; i < cards.Count; i++)
        {
            if (cardToPoints[cards[i]] > cardToPoints[other.cards[i]])
            {
                return 1;
            }
            if (cardToPoints[cards[i]] < cardToPoints[other.cards[i]])
            {
                return -1;
            }
        }

        return 0;
    }
}


[DebuggerDisplay("{AsString}")]
public class HandPart2 : IComparable<HandPart2>
{
    public int Bid { get; }
    public readonly List<char> cards;
    public string AsString => string.Join("", cards);

    public override string ToString()
    {
        return AsString;
    }

    public Dictionary<char, int> cardToPoints = new()
    {
        { 'A', 14 },
        { 'K', 13 },
        { 'Q', 12 },
        { 'T', 10 },
        { '9', 9 },
        { '8', 8 },
        { '7', 7 },
        { '6', 6 },
        { '5', 5 },
        { '4', 4 },
        { '3', 3 },
        { '2', 2 },
        { 'J', 1 },
    };

    public HandPart2(List<char> cards, int bid)
    {
        Bid = bid;
        this.cards = cards;
    }

    public int GetStrength()
    {
        if (cards.Distinct().Count() == 1)
        {
            return 7;
        }

        if (cards.Contains('J'))
        {
            foreach (var c in cards)
            {
                var xxx = string.Join("", cards).Replace('J', c);
                if (xxx.Distinct().Count() == 1)
                {
                    return 7;
                }
            }
        }

        if (cards.Any(c => cards.Count(x => x == c) == 4))
        {
            return 6;
        }

        if (cards.Contains('J'))
        {
            foreach (var c in cards)
            {
                var xxx = string.Join("", cards).Replace('J', c);
                if (xxx.Any(j => xxx.Count(x => x == j) == 4))
                {
                    return 6;
                }
            }
        }
        
        if (cards.Any(c => cards.Count(x => x == c) == 3) && cards.Any(c => cards.Count(x => x == c) == 2))
        {
            return 5;
        }
        
        if (cards.Contains('J'))
        {
            foreach (var c in cards)
            {
                var xxx = string.Join("", cards).Replace('J', c);
                if (xxx.Any(c => xxx.Count(x => x == c) == 3) && xxx.Any(j => xxx.Count(x => x == j) == 2))
                {
                    return 5;
                }
            }
        }

        if (cards.Any(c => cards.Count(x => x == c) == 3))
        {
            return 4;
        }

        if (cards.Contains('J'))
        {
            foreach (var c in cards)
            {
                var xxx = string.Join("", cards).Replace('J', c);
                if (xxx.Any(c => xxx.Count(x => x == c) == 3))
                {
                    return 4;
                }
            }
        }

        var pairs = cards.Where(c => cards.Count(x => x == c) == 2).Distinct().ToList();
        if (pairs.Count == 2)
        {
            return 3;
        }

        if (cards.Contains('J'))
        {
            foreach (var c in cards)
            {
                var xxx = string.Join("", cards).Replace('J', c);
                var newPairs = xxx.Where(c => xxx.Count(x => x == c) == 2).Distinct().ToList();
                if (newPairs.Count == 2)
                {
                    return 3;
                }

                if (newPairs.Count == 1)
                {
                    return 2;
                }
            }
        }
        
        if (pairs.Count == 1)
        {
            return 2;
        }

        return 1;
    }

    public int CompareTo(HandPart2 other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (ReferenceEquals(null, other)) return 1;

        var strength = GetStrength();
        var otherStrength = other.GetStrength();

        if (strength > otherStrength)
        {
            return 1;
        }

        if (strength < otherStrength)
        {
            return -1;
        }

        for (int i = 0; i < cards.Count; i++)
        {
            if (cardToPoints[cards[i]] > cardToPoints[other.cards[i]])
            {
                return 1;
            }
            if (cardToPoints[cards[i]] < cardToPoints[other.cards[i]])
            {
                return -1;
            }
        }

        return 0;
    }
}