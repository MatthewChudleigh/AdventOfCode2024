namespace A20;

public static class Solution
{
    public class Track
    {
        public (int X, int Y) Start { get; set; }
        public (int X, int Y) End { get; set; }
        public Dictionary<(int X, int Y), int> Points { get; set; } = new();
        public HashSet<(int X, int Y)> Walls { get; set; } = new();

        private static readonly List<(int X, int Y)> Off = [(-1,0), (1, 0), (0,-1), (0, 1)];
        public void Calc()
        {
            var test = new Stack<(int X, int Y, int Cost)>();
            test.Push((Start.X, Start.Y, 0));
            while (test.TryPop(out var pos))
            {
                Points[(pos.X, pos.Y)] = pos.Cost;
                
                if ((pos.X, pos.Y) == End)
                {
                    break;
                }

                foreach (var t in Off.Select(o => (X: pos.X + o.X, Y: pos.Y + o.Y))
                             .Where(p => Points.TryGetValue(p, out var c) && (c < 0 || pos.Cost+1 < c)))
                {
                    test.Push((t.X, t.Y, pos.Cost + 1));
                }
            }
        }

        public Dictionary<int, int> Cheats(int maxMoves)
        {
            var costs = new Dictionary<int, int>();
            foreach (var (xy, start) in Points)
            {
                var visited = new Dictionary<(int X, int Y), int>();
                var paths = new Stack<((int X, int Y) Pos, int M)>();

                foreach (var o1 in Off)
                {
                    var pos1 = (xy.X + o1.X, xy.Y + o1.Y);
                    paths.Push((pos1, 1));
                }
                
                while (paths.TryPop(out var path))
                {
                    if (path.M > maxMoves) continue;
                    if (visited.TryGetValue(path.Pos, out var m) && path.M <= m) continue;
                    visited[path.Pos] = path.M;
                    
                    if (Points.TryGetValue(path.Pos, out var end) && start < end)
                    {
                        costs.TryGetValue(end - (start + path.M), out var c);
                        costs[end - (start + path.M)] = ++c;
                    }
                    else if (Walls.Contains(path.Pos))
                    {
                        foreach (var o1 in Off)
                        {
                            var pos1 = (path.Pos.X + o1.X, path.Pos.Y + o1.Y);
                            paths.Push((pos1, path.M + 1));
                        }
                    }
                }
            }

            return costs;
        }
    }

    public static Track Load(string[] data)
    {
        var track = new Track();
        var y = 0;
        foreach (var line in data)
        {
            var x = 0;
            foreach (var c in line)
            {
                if (c != '#')
                {
                    if (c == 'S')
                    {
                        track.Start = (x, y);
                    }
                    else if (c == 'E')
                    {
                        track.End = (x, y);
                    }

                    track.Points[(x, y)] = -1;
                }
                else
                {
                    track.Walls.Add((x, y));
                }
                

                x++;
            }

            y++;
        }

        return track;
    }
}