using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AdventOfCode;
using AdventOfCode.Utils;

namespace AoC2023
{
    public class Day17 : IAocDay
    {
        public async Task<object> Part1()
        {
            var input = await IO.GetInput(2023, 17);
            //var input = await IO.GetExampleInput();
            var grid = input.Select(x => x.ToCharArray().Select(c => int.Parse($"{c}")).ToArray()).ToArray();
            var evaluated = new Dictionary<State, int>();
            var Q = new PriorityQueue<State, int>();
            var destination = new Point(grid.Length - 1, grid[0].Length - 1);
            var initialState = new State()
            {
                pos = new Point(0, 0),
                dir = new Point(0, 1),
                movesLeft = 3,
            };

            Q.Enqueue(initialState, 0);
            while (Q.Count > 0)
            {
                if (!Q.TryDequeue(out var state, out var totalHeat))
                {
                    throw new Exception();
                }

                if (state.pos.Equals(destination))
                {
                    return totalHeat;
                }

                if (evaluated.ContainsKey(state))
                {
                    continue;
                }

                evaluated[state] = totalHeat;

                foreach (var candidate in GenerateCandidates(state).Where(x => Misc.PointInGrid(x.pos, grid)))
                {
                    var candidateHeat = grid[candidate.pos.x][candidate.pos.y];
                    Q.Enqueue(candidate, totalHeat + candidateHeat);
                }
            }

            throw new Exception("Should have gotten an answer by now");
        }

        public async Task<object> Part2()
        {
            var input = await IO.GetInput(2023, 17);
            //var input = await IO.GetExampleInput();
            var grid = input.Select(x => x.ToCharArray().Select(c => int.Parse($"{c}")).ToArray()).ToArray();
            var evaluated = new Dictionary<State, int>();
            var Q = new PriorityQueue<State, int>();
            var destination = new Point(grid.Length - 1, grid[0].Length - 1);
            var initialState = new State()
            {
                pos = new Point(0, 0),
                dir = new Point(0, 1),
                movesLeft = 10,
            };

            var initialState2 = new State()
            {
                pos = new Point(0, 0),
                dir = new Point(1, 0),
                movesLeft = 10,
            };

            Q.Enqueue(initialState, 0);
            Q.Enqueue(initialState2, 0);
            while (Q.Count > 0)
            {
                if (!Q.TryDequeue(out var state, out var totalHeat))
                {
                    throw new Exception();
                }

                if (state.pos.Equals(destination))
                {
                    return totalHeat;
                }

                if (evaluated.ContainsKey(state))
                {
                    continue;
                }

                evaluated[state] = totalHeat;

                foreach (var candidate in GenerateCandidates2(state).Where(x => Misc.PointInGrid(x.pos, grid)))
                {
                    var candidateHeat = grid[candidate.pos.x][candidate.pos.y];
                    if (candidate.pos.Equals(destination) && candidate.movesLeft > 6)
                    {
                        continue;
                    }

                    Q.Enqueue(candidate, totalHeat + candidateHeat);
                }
            }

            throw new Exception("Should have gotten an answer by now");
        }

        public IEnumerable<State> GenerateCandidates(State state)
        {
            if (state.movesLeft > 0)
            {
                yield return new State
                {
                    pos = state.pos + state.dir,
                    dir = state.dir,
                    movesLeft = state.movesLeft - 1,
                };
            }

            yield return new State
            {
                pos = state.pos + state.dir.TurnLeft(),
                dir = state.dir.TurnLeft(),
                movesLeft = 2,
            };

            yield return new State
            {
                pos = state.pos + state.dir.TurnRight(),
                dir = state.dir.TurnRight(),
                movesLeft = 2,
            };
        }

        public IEnumerable<State> GenerateCandidates2(State state)
        {
            if (state.movesLeft > 0)
            {
                yield return new State
                {
                    pos = state.pos + state.dir,
                    dir = state.dir,
                    movesLeft = state.movesLeft - 1,
                };
            }

            if (state.movesLeft <= 6)
            {
                yield return new State
                {
                    pos = state.pos + state.dir.TurnLeft(),
                    dir = state.dir.TurnLeft(),
                    movesLeft = 9,
                };

                yield return new State
                {
                    pos = state.pos + state.dir.TurnRight(),
                    dir = state.dir.TurnRight(),
                    movesLeft = 9,
                };
            }
        }

        [DebuggerDisplay("{Debug}")]
        public struct State
        {
            public Point pos;
            public Point dir;
            public int movesLeft;

            public string Debug => $"Pos: {pos.x},{pos.y} ; Dir: {dir.x},{dir.y} ; MovesLeft: {movesLeft} ; ";

            public override bool Equals(object obj)
            {
                if (obj is not State s)
                {
                    return false;
                }

                return pos.Equals(s.pos) && dir.Equals(s.dir) && movesLeft.Equals(s.movesLeft);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return pos.GetHashCode() ^ dir.GetHashCode() ^ movesLeft.GetHashCode();
                }
            }
        }
    }
}
