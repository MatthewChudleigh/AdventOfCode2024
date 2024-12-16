using System.Text;

namespace A15;

public static class Solution
{
    public class Points : Dictionary<(int X, int Y), char>
    {
        public bool Explore((int X, int Y) next, (int X, int Y) move)
        {
            var newPoints = new Points();
            var hasChecked = new HashSet<(int X, int Y)>();
            var check = new Stack<(int X, int Y)>();
            check.Push(next);
            while (check.Any())
            {
                var p = check.Pop();
                if(!hasChecked.Add(p)) continue;

                newPoints.TryAdd(p, '.');
                
                if (move.Y != 0)
                {
                    if (this[p] == ']')
                    {
                        var p1 = p with { X = p.X - 1 };
                        check.Push(p1);
                    }

                    if (this[p] == '[')
                    {
                        var p1 = p with { X = p.X + 1 };
                        check.Push(p1);
                    }
                }
                
                if (TryFindSpace(p, move, out var points))
                {
                    for (var i = 1; i < points.Count; ++i)
                    {
                        var cell1 = this[points[i - 1]];
                        var cell0 = this[points[i]];
                        
                        newPoints[points[i]] = cell1;                     
                        if (move.Y != 0 && ((cell0 == ']' && cell1 == '[') || (cell0 == '[' && cell1 == ']')))
                        {
                            check.Push(points[i]);
                        }
                    }
                }
                else
                {
                    return false;
                }
            }

            foreach (var p in newPoints)
            {
                this[p.Key] = p.Value;
            }
            
            return true;
        }

        public bool TryFindSpace((int X, int Y) point, (int X, int Y) move, out List<(int X, int Y)> points)
        {
            points = [point];
            var next = (X: point.X + move.X, Y: point.Y + move.Y);
            while (TryGetValue(next, out var cell))
            {
                points.Add(next);
                if (cell == '#') return false;
                if (cell == '.') break;
                    
                next = (next.X + move.X, next.Y + move.Y);
            }

            return points.Count != 0;
        }
    }
    
    public class Map
    {
        public int Width { get; set;  }
        public int Height { get; set; }
        
        public (int X, int Y) Robot { get; set; }
        public Points Points { get; } = new();

        public string Render()
        {
            var sb = new StringBuilder();
            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    if (Robot == (x, y))
                    {
                        sb.Append('@');
                    }
                    else
                    {
                        Points.TryGetValue((x, y), out var c);
                        sb.Append(c);
                    }
                }
                
                sb.AppendLine();
            }

            return sb.ToString();
        }
        
        public IEnumerable<(int X, int Y)> Apply(string moves)
        {
            foreach (var move in MovesToDir(moves))
            {
                var next = (X: Robot.X + move.X, Y: Robot.Y + move.Y);
                if (!Points.TryGetValue(next, out var cell0) || cell0 == '#') continue;
                if (cell0 == '.')
                {
                    Robot = next;
                }
                else if (Points.Explore(next, move))
                {
                    Robot = next;
                    Points[next] = '.';
                }

                yield return move;
            }

            yield return (0,0);
        }

    }

    
    public static int Calculate(Map map)
    {
        var sum = 0;
        foreach (var box in map.Points.Where(kv => kv.Value == 'O' || kv.Value == '0' || kv.Value == '['))
        {
            sum += box.Key.X + 100 * box.Key.Y;
        }
        return sum;
    }

    public static Map LinesToMap(string[] mapLines, int scale)
    {
        var map = new Map();
        var y = 0;
        foreach (var line in mapLines)
        {
            var x = 0;
            foreach (var c in line)
            {
                var p = c;
                if (p == '@')
                {
                    map.Robot = (x, y);
                    p = '.';
                }

                for (var s = 0; s < scale; s++)
                {
                    if (p != '.' && p != '#' && scale == 2) p = (s == 0 ? '[' : ']');
                    map.Points[(x, y)] = p;
                    x++;
                }
            }
            map.Width = map.Width < x ? x : map.Width; 

            y++;
        }
        map.Height = y;

        return map;
    }

    public static IEnumerable<(int X, int Y)> MovesToDir(string moves)
    {
        foreach (var move in moves)
        {
            switch (move)
            {
                case '<': 
                    yield return (-1, 0);
                    break;
                case '^': 
                    yield return (0, -1);
                    break;
                case '>': 
                    yield return (1, 0);
                    break;
                case 'v': 
                    yield return (0, 1);
                    break;
            }
        }
    }
}