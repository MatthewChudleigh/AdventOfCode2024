using Common;

var dataPath = "/workspaces/AdventOfCode2024/data/A06/A06.1.txt";

var result = A06.VisitCount(dataPath);
Console.WriteLine(result);

static class A06
{
    public class Path : Dictionary<(int X, int Y), HashSet<int>> {}

    public class Map
    {
        public HashSet<(int X, int Y)> Obstacles { get; set; } = new HashSet<(int X, int Y)>();
        public int Width { get; set; }
        public int Height { get; set; }
    }

    public static (int Visits, int PotentialObstructions) VisitCount(string dataPath)
    {
        var map = new Map();

        var dir = 0;
        var y = 0;

        (int X, int Y) startingPos = (0, 0);

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
                        startingPos = (x, y);
                        break;
                }
                x++;
            }
            map.Width = x;
            y++;
        }
        map.Height = y;

        var (_, guardPath) = MapPath(map, dir, startingPos);

        var potentialObstructions = 0;

        foreach (var extraObstacle in guardPath.Keys)
        {
            if (extraObstacle == startingPos) continue;
            var (looped, _) = MapPath(map, dir, startingPos, extraObstacle);

            if (looped)
            {
                potentialObstructions++;
            }
        }

        return (guardPath.Count(), potentialObstructions);
    }

    private static List<(int X, int Y)> Directions = new List<(int X, int Y)>{
        (0, -1), (1, 0), (0, 1), (-1, 0)
    };
    private static (bool Looped, Path Path) MapPath(Map map, int dir, (int X, int Y) pos, (int X, int Y)? extraObstacle = null)
    {
        var path = new Path();
        path.AddToSet(pos, dir);

        var steps = 0;
        while (pos.X >= 0 && pos.X < map.Width && pos.Y >= 0 && pos.Y < map.Height)
        {
            var d = Directions[dir];
            var nextPos = (pos.X + d.X, pos.Y + d.Y);
            if (path.TryGetValue(nextPos, out var dirs) && dirs.Contains(dir))
            {   // Looped
                return (true, path);
            }
            else if (map.Obstacles.Contains(nextPos) || extraObstacle == nextPos)
            {   // Hit wall
                dir = (dir + 1) % 4;
            }
            else
            {   // Record guard path
                path.AddToSet(pos, dir);
                pos = nextPos;
            }
            steps++;
        }

        return (false, path);
    }
}