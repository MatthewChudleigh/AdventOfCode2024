var dataPath = "/workspaces/AdventOfCode2024/data/A06/A06.1.txt";

var visitCount = A06.VisitCount(dataPath);
Console.WriteLine(visitCount);

static class A06
{
    public class Map
    {
        public HashSet<(int X, int Y)> GuardPath { get; set; } = new HashSet<(int X, int Y)>();
        public HashSet<(int X, int Y)> Obstacles { get; set; } = new HashSet<(int X, int Y)>();
        public int Width { get; set; }
        public int Height { get; set; }
    }

    public static int VisitCount(string dataPath)
    {
        var map = new Map();

        (int X, int Y) pos = (0, 0);

        var y = 0;
        foreach (var line in File.ReadAllLines(dataPath))
        {
            var x = 0;
            foreach (var c in line)
            {
                switch (c)
                {
                    case '#':
                        map.Obstacles.Add((x, y));
                        break;
                    case '^':
                        map.GuardPath.Add((x, y));
                        pos = (x, y);
                        break;
                }
                x++;
            }
            map.Width = x;
            y++;
        }
        map.Height = y;

        var dir = 0;
        var directions = new List<(int X, int Y)>{
            (0, -1), (1, 0), (0, 1), (-1, 0)
        };

        var stepsTotal = 0;
        while (pos.X >= 0 && pos.X < map.Width && pos.Y >= 0 && pos.Y < map.Height)
        {
            var d = directions[dir % 4];
            var nextPos = (pos.X + d.X, pos.Y + d.Y);
            if (map.Obstacles.Contains(nextPos))
            {
                dir++;
            }
            else
            {
                stepsTotal++;
                map.GuardPath.Add(pos);
                pos = nextPos;
            }
        }

        Console.WriteLine(pos);
        Console.WriteLine(stepsTotal);

        return map.GuardPath.Count();
    }
}