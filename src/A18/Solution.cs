namespace A18;

public class Solution
{
    public class Map
    {
        public int Width { get; set;  }
        public int Height { get; set; }

        public Dictionary<(int X, int Y), int> Walls { get; set; } = new();
    }

    
    public static int? Solve(int width, int height, string[] walls, int wallCount)
    {
        var choices = new List<(int X, int Y)>()
        {
            (1, 0), (-1, 0), (0, 1), (0, -1)
        };
        var map = new Map()
        {
            Width = width,
            Height = height
        };

        var t = 0;
        foreach (var wall in walls.Select(w => w.Split(",").Select(Int32.Parse).ToList()))
        {
            map.Walls.TryAdd((wall[0], wall[1]), 0);
            t++;
            if (wallCount == t) break;
        }

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
            
            if (path.X == width - 1 && path.Y == height - 1)
            {
                minSteps = path.Steps <= (minSteps ?? path.Steps) ? path.Steps : minSteps;  
                continue;
            }
            
            foreach (var choice in choices.Select(xy => (X: path.X + xy.X, Y: path.Y + xy.Y))
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