using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace AdventOfCode.Utils;

// both ends are inclusive
[DebuggerDisplay("{s}-{e}")]
public struct Range
{
    public ulong s;
    public ulong e;

    public Range(ulong s, ulong e)
    {
        this.s = s <= e ? s : e;
        this.e = e >= s ? e : s;
    }

    public bool Overlaps(Range other)
    {
        return s <= other.e && e >= other.s;
    }

    public static Range operator +(Range a, Range b) => a.Overlaps(b)
        ? new Range { s = Math.Min(a.s, b.s), e = Math.Max(a.e, b.e) }
        : throw new Exception("Ranges don't overlap");

    public Range Intersect(Range other)
    {
        return Overlaps(other)
            ? new Range (Math.Max(s, other.s), Math.Min(e, other.e))
            : throw new Exception("Ranges don't overlap");
    }

    public IEnumerable<ulong> Enumerate()
    {
        for (ulong i = s; i <= e; i++)
        {
            yield return i;
        }
    }

    public bool Contains(ulong item)
    {
        return (s <= item) && (e >= item);
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