using System.Text;

namespace A15;

public static class Solution
{
    public class Map
    {
        public int Width { get; set;  }
        public int Height { get; set; }
        
        public (int X, int Y) Robot { get; set; }
        public Dictionary<(int X, int Y), char> Points { get; } = new();

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
        
        public void Apply(string moves)
        {
            foreach (var move in MovesToDir(moves))
            {
                var next = (X: Robot.X + move.X, Y: Robot.Y + move.Y);
                if (!Points.TryGetValue(next, out var point) || point == '#') continue;
                if (point != 'O')
                {
                    Robot = next;
                    continue;
                }

                var spaceFound = false;
                var end = (X: next.X + move.X, Y: next.Y + move.Y);
                while (Points.TryGetValue(end, out point))
                {
                    if (point == '#') break;
                    if (point != 'O')
                    {
                        spaceFound = true;
                        break;
                    }
                    end = (end.X + move.X, end.Y + move.Y);
                }

                if (spaceFound)
                {
                    Robot = next;
                    Points[next] = '.';
                    Points[end] = 'O';
                }
            
            }
        }

    }
    
    public static int Calculate(Map map)
    {
        var sum = 0;
        foreach (var box in map.Points.Where(kv => kv.Value == 'O'))
        {
            sum += box.Key.X + 100 * box.Key.Y;
        }
        return sum;
    }

    public static Map LinesToMap(string[] mapLines)
    {
        var map = new Map();
        var y = 0;
        foreach (var line in mapLines)
        {
            var x = 0;
            foreach (var c in line)
            {
                if (c == '@')
                {
                    map.Points[(x, y)] = '.';
                    map.Robot = (x, y);
                }
                else
                {
                    map.Points[(x, y)] = c;
                }
                
                x++;
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