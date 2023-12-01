namespace AdventOfCode.Utils
{
    public struct Point
    {
        public int x;
        public int y;

        public static Point[] GetDirs(bool includeDiagonals)
        {
            if (includeDiagonals)
            {
                return new Point[]
                {
                    new() { x = 1, y = 0 },
                    new() { x = 1, y = 1 },
                    new() { x = 0, y = 1 },
                    new() { x = 1, y = 1 },
                    new() { x = -1, y = 0 },
                    new() { x = -1, y = -1 },
                    new() { x = 0, y = -1 },
                    new() { x = -1, y = -1 },
                };
            }
            else
            {
                return new Point[]
                {
                    new() { x = 1, y = 0 },
                    new() { x = 0, y = 1 },
                    new() { x = -1, y = 0 },
                    new() { x = 0, y = -1 },
                };
            }
        }
    }
}
