namespace A16;

public static class Solution
{
    public class Map
    {
        public Dictionary<(int X, int Y), char> Points { get; } = new();
        public int Width { get; set; }
        public int Height { get; set; }
        public int? MinScore { get; set; }
        public (int X, int Y) Start { get; set; }
        public (int X, int Y) End { get; set; }
        public HashSet<(int X, int Y)> Track { get; } = new();
        public Dictionary<(int X, int Y, int Dir), (int? Cost, HashSet<Path> Parents)>  Weights { get; } = new();
    }

    public record Path((int X, int Y) Position, int Dir, int Cost);

    public static List<(int X, int Y)> Dirs = [(1,0), (0, 1), (-1, 0), (0, -1)];

    public static (int? MinScore, int? BestSeats) CalculateMinScore(Map map)
    {
        var paths = new Stack<Path>();
        paths.Push(new Path(map.Start, 0, 0));
        map.Weights[(map.Start.X, map.Start.Y, 0)] = (0, []);

        var endDir = new List<int>();
        while (paths.TryPop(out var path))
        {
            if (map.End == (path.Position.X, path.Position.Y))
            {
                if (path.Cost <= (map.MinScore ?? path.Cost))
                {
                    if (path.Cost < (map.MinScore ?? path.Cost))
                    {
                        endDir.Clear();
                    } 
                    endDir.Add(path.Dir);
                    map.MinScore = path.Cost;
                }
                continue;
            }
            
            var choices = (new List<(int Dir, int Cost)>()
                { (path.Dir, 1), (path.Dir == 0 ? 3 : path.Dir - 1, 1001), (path.Dir == 3 ? 0 : path.Dir + 1, 1001) })
                .Select(choice => new Path(
                    Position: (path.Position.X + Dirs[choice.Dir].X, path.Position.Y + Dirs[choice.Dir].Y), 
                    Dir: choice.Dir, 
                    Cost: path.Cost + choice.Cost))
                .Where(p => map.Track.Contains(p.Position)).ToList();
            foreach (var choice in choices)
            {
                Test(map, paths, path, choice);
            }
        }
        
        foreach (var d in endDir)
        {
            paths.Push(new Path(map.End, d, 0));
        }

        var bestSeats = new HashSet<(int X, int Y)>();
        
        while (paths.TryPop(out var path))
        {
            bestSeats.Add((path.Position.X, path.Position.Y));
            var w = map.Weights[(path.Position.X, path.Position.Y, path.Dir)];
            foreach (var p in w.Parents)
            {
                paths.Push(p);
            }
        }
        
        return (map.MinScore, bestSeats.Count);
    }

    public static void Test(Map map, Stack<Path> paths, Path parentPath, Path path)
    {
        var (x, y) = (path.Position.X, path.Position.Y);

        var key = (x, y, path.Dir);
        if (!map.Weights.TryGetValue(key, out var w))
        {
            w = (null, []);
        }

        if (!w.Cost.HasValue || path.Cost < w.Cost)
        {
            w.Parents.Clear();
            
            paths.Push(path);
        }
        
        if (!w.Cost.HasValue || path.Cost <= w.Cost)
        {
            w.Parents.Add(parentPath);
            map.Weights[key] = w with { Cost = path.Cost };
        }
    }
    
    public static Map LinesToMap(string[] mapLines)
    {
        var map = new Map() { };
        var y = 0;
        foreach (var line in mapLines)
        {
            var x = 0;
            foreach (var c in line)
            {
                if (c == 'S')
                {
                    map.Start = (x, y);
                }
                else if (c == 'E')
                {
                    map.End = (x, y);
                    map.Track.Add((x, y));
                }

                if (c == '.')
                {
                    map.Track.Add((x, y));
                }

                map.Points[(x, y)] = c;
                x++;
            }
            map.Width = map.Width < x ? x : map.Width; 

            y++;
        }
        map.Height = y;

        return map;
    }
}