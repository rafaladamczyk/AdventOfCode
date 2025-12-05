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
            this.s = s;
            this.e = e;
        }
        else
        {
            this.s = e;
            this.e = s;
        }
    }

    public ulong Length => e - s;

    public Range? Intersect(Range other)
    {
        if (e < other.s)
        {
            return null;
        }

        if (s > other.e)
        {
            return null;
        }

        return new Range(other.s, e);
    }

    public static Range? operator +(Range a, Range b) =>
        a.Intersect(b) != null ? new Range(Math.Min(a.s, b.s), Math.Max(a.e, b.e)) : null;

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