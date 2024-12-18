namespace A18;

public class Solution
{
    public class Map
    {
        public int Width { get; set;  }
        public int Height { get; set; }

        public Dictionary<(int X, int Y), int> Walls { get; set; } = new();
    }

    
    public static int? Solve(int width, int height, (int X, int Y)[] walls, int wallCount)
    {
        var map = new Map()
        {
            Width = width,
            Height = height
        };

        var t = 0;
        while (t < wallCount)
        {
            map.Walls.TryAdd((walls[t].X, walls[t].Y), 0);
            t++;
        }

        int? minSteps = null;
        while (t <= walls.Length)
        {
            minSteps = Solve(map);
            if (minSteps == null)
            {
                Console.WriteLine(walls[t-1]);
                break;
            }

            map.Walls.TryAdd((walls[t].X, walls[t].Y), 0);
            t++;
        }
        
        return minSteps;
    }

    public static readonly List<(int X, int Y)> Choices = [(1, 0), (-1, 0), (0, 1), (0, -1)];
    public static int? Solve(Map map)
    {
        var visited = new Dictionary<(int X, int Y), int>();
        var paths = new Stack<(int X, int Y, int Steps)>();

        int? minSteps = null;
        paths.Push((0, 0, 0));

        while (paths.TryPop(out var path))
        {
            if (visited.TryGetValue((path.X, path.Y), out var v) && v <= path.Steps)
            {
                continue;
            }

            visited[(path.X, path.Y)] = path.Steps;
            
            if (path.X == map.Width - 1 && path.Y == map.Height - 1)
            {
                minSteps = path.Steps <= (minSteps ?? path.Steps) ? path.Steps : minSteps;  
                continue;
            }
            
            foreach (var choice in Choices.Select(xy => (X: path.X + xy.X, Y: path.Y + xy.Y))
                         .Where(xy => 0 <= xy.X && xy.X < map.Width)
                         .Where(xy => 0 <= xy.Y && xy.Y < map.Height)
                         .ToList())
            {
                if (!map.Walls.TryGetValue(choice, out var tx) || tx > path.Steps + 1)
                {
                    paths.Push((choice.X, choice.Y, path.Steps + 1));
                }
            }
        }

        return minSteps;
    }
}