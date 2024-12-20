namespace A20;

public static class Solution
{
    public class Track
    {
        public (int X, int Y) Start { get; set; }
        public (int X, int Y) End { get; set; }
        public Dictionary<(int X, int Y), int> Points { get; } = new();

        private static readonly List<(int X, int Y)> Off = [(0,-1), (-1,0), (1, 0), (0, 1)];
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
            var end = Points[End];
            var cheats = new Dictionary<int, int>();
            var points = Points.OrderBy(kv => kv.Value).Select(kv => kv.Key).ToList();
            for (var i = 0; i < points.Count - 1; ++i)
            {   // For each step along the path
                for (var j = i + 1; j < points.Count; ++j)
                {   // to each possible end point
                    // Find the shortest distance between the two points
                    var dist = Math.Abs((points[j].X - points[i].X)) + Math.Abs((points[j].Y - points[i].Y));
                    var cheat = (j - (i + dist));
                    if (dist <= maxMoves && cheat >= 100)
                    {   // If the cheat distance does not exceed the maximum allowed,
                        // and it improves the time by >= 100
                        // record it
                        cheats.TryGetValue(cheat, out var d);
                        cheats[cheat] = d + 1;
                    }
                }
            }

            return cheats;
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

                x++;
            }

            y++;
        }

        return track;
    }
}