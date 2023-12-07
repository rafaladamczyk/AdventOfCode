using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace AdventOfCode.Utils;

// inclusive start, exclusive end
[DebuggerDisplay("[{s}-{e})")]
public struct Range
{
    public ulong s;
    public ulong e;

    public Range(ulong s, ulong e)
    {
        if (s < e)
        {
            this.s = 0;
            this.e = 0;
        }
        else
        {
            this.s = s;
            this.e = e;
        }
    }

    public ulong Length => e - s;

    public Range Intersect(Range other) => new Range(Math.Max(s, other.s), Math.Min(e, other.e));
    
    public static List<Range> operator +(Range a, Range b) => a.Intersect(b).Length > 0
        ? new List<Range>() { new(Math.Min(a.s, b.s), Math.Max(a.e, b.e)) }
        : new List<Range>() { a, b };

    public IEnumerable<ulong> Enumerate()
    {
        for (ulong i = s; i < e; i++)
        {
            yield return i;
        }
    }

    public bool Contains(ulong item)
    {
        return (s <= item) && (e > item);
    }

    public override bool Equals(object obj)
    {
        if (obj is not Range p)
        {
            return false;
        }

        return p.s == s && p.e == e;
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return s.GetHashCode() ^ e.GetHashCode();
        }
    }
}