namespace A20;

public static class Solution
{
    public class Track
    {
        public (int X, int Y) Start { get; set; }
        public (int X, int Y) End { get; set; }
        public Dictionary<(int X, int Y), int> Points { get; set; } = new();
        public HashSet<(int X, int Y)> Walls { get; set; } = new();

        private List<(int X, int Y)> Off = [(-1,0), (1, 0), (0,-1), (0, 1)];
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
                else
                {
                    foreach (var t in Off.Select(o => (X: pos.X + o.X, Y: pos.Y + o.Y))
                                 .Where(p => Points.TryGetValue(p, out var c) && (c < 0 || pos.Cost+1 < c)))
                    {
                        test.Push((t.X, t.Y, pos.Cost + 1));
                    }
                }
            }
        }

        public Dictionary<int, int> Cheats()
        {
            var endCost = Points[End];
            var costs = new Dictionary<int, int>();
            foreach (var (xy, cost1) in Points)
            {
                foreach (var o1 in Off)
                {
                    var w = (xy.X + o1.X, xy.Y + o1.Y);
                    if (!Walls.Contains(w)) continue;
                    
                    foreach (var o2 in Off)
                    {
                        var nP = (xy.X + o1.X + o2.X, xy.Y + o1.Y + o2.Y);
                        if(Points.TryGetValue(nP, out var cost2) && cost2 > (cost1 + 2))
                        {
                            // E = 84
                            // P1 = 33
                            // P2 = 45
                            // 84-(45-33)
                            var newCost = (cost2 - (cost1 + 2));
                            costs.TryGetValue(newCost, out var c);
                            costs[newCost] = ++c;
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