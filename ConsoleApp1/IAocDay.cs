using System.Threading.Tasks;

namespace AdventOfCode;

public interface IAocDay
{
    Task<object> Part1();
    Task<object> Part2();
}

