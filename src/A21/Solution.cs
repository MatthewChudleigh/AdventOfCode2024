using System.Text;

namespace A21;

public static class Solution
{
    public static Keypad Keypad = new Keypad();
    public static Arrows Arrows = new Arrows();
    
    public static (string moves, long total) Calculate(char start, string code, int robots)
    {
        var moves = GetAllMoves(Keypad, start, code, robots);
        long.TryParse(code[..^1], out var val);
        return (moves.moves, total: moves.total * val);
    }
    
    public static (string moves, long total) GetAllMoves(InputDevice input, char start, string code, int robots)
    {
        var total = 0L;

        var pos = input.Keys[start];
        var allMoves = new StringBuilder();
        foreach (var c in code)
        {
            var end = input.Keys[c];
            
            var dx = pos.X < end.X ? 1 : pos.X > end.X ? -1 : 0;
            var dy = pos.Y < end.Y ? 1 : pos.Y > end.Y ? -1 : 0;

            if ((dx != 0 && Costs.TryGetValue((pos, (X: dx, Y: 0), end, robots), out var cost))
                || (dy != 0 && Costs.TryGetValue((pos, (X: 0, Y: dy), end, robots), out cost)))
            {
                total += cost;
                pos = end;
                continue;
            }
            
            var toVisit = new PriorityQueue<((int X, int Y) Pos, (int X, int Y) Dir, StringBuilder Moves), int>();

            if (dx != 0)
            {
                toVisit.Enqueue(((X: pos.X+dx, pos.Y), (dx, 0), new StringBuilder($"{start}")), 0);
            }
            if (dy != 0)
            {
                toVisit.Enqueue(((X: pos.X, Y: pos.Y + dy), (0, dy), new StringBuilder($"{start}")), 0);
            }

            var current = 0L;
            var visited = new HashSet<(int X, int Y)>();
            
            while (toVisit.TryDequeue(out var cur, out var dist))
            {
                if (!input.Map.TryGetValue(cur.Pos, out var n)) continue;
                if (cur.Pos == end)
                {
                    var moves = cur.Moves.ToString();
                    if (robots == 0)
                    {
                        current += moves.Length;
                        allMoves.Append(moves);
                    }
                    else
                    {
                        var x = new StringBuilder(moves);
                        x.Append(c);
                        var (m, cost1) = GetAllMoves(Arrows, c, moves, robots - 1);
                        current += cost1;
                    }
                    break;
                }

                if (!visited.Add(cur.Pos)) continue;
                
                dx = cur.Pos.X < end.X ? 1 : cur.Pos.X > end.X ? -1 : 0;
                dy = cur.Pos.Y < end.Y ? 1 : cur.Pos.Y > end.Y ? -1 : 0;

                var next = new StringBuilder(cur.Moves.ToString());
                next.Append(c);
                
                if (dx != 0)
                {
                    toVisit.Enqueue(((X: cur.Pos.X + dx, Y: cur.Pos.Y), (dx, 0), next), dist + 1);
                }
                if (dy != 0)
                {
                    toVisit.Enqueue(((X: cur.Pos.X, Y: cur.Pos.Y + dy), (0, dy), next), dist + 1);
                }
            }

            total += current;
            pos = end;
        }
        
        return (allMoves.ToString(), total);
    }

    public static Dictionary<((int X, int Y) Pos, (int X, int Y) Dir, (int X, int Y) Tgt, int Robot), int> Costs = new();
}

public class InputDevice
{
    public char Pos { get; set; } = 'A';
    public Dictionary<char, (int X, int Y)> Keys = new();
    public Dictionary<(int X, int Y), char> Map = new();
    public Dictionary<(char From, char To), string> Moves = new();

    public void Init()
    {
        foreach (var move in Moves.ToList())
        {
            Moves[(move.Key.To, move.Key.From)] = move.Value switch
            {
                "<" => ">",
                "v" => "^",
                "^" => "v",
                _ => move.Value
            };
        }
        
        foreach (var key in Keys)
        {
            Map[key.Value] = key.Key;
        }
    }
}

public class Arrows : InputDevice
{
    public Arrows()
    {
        Keys = new Dictionary<char, (int X, int Y)>
        {
            { '^', (-1, 0) },
            { 'A', (0, 0) },
            { '<', (-2, -1) },
            { 'v', (-1, -1) },
            { '>', (0, -1) },
        };
        Moves = new Dictionary<(char From, char To), string>
        {
            {('A', '^'), "<"},
            {('A', '>'), "v"},
            {('^', 'v'), "v"},
            {('v', '<'), "<"},
            {('>', 'v'), "<"},
        };
        Init();
    }
}

public class Keypad : InputDevice
{
    public Keypad()
    {
        Keys = new Dictionary<char, (int X, int Y)>
        {
            { 'A', (0, 0) },
            { '0', (-1, 0) },
            { '3', (0, 1) },
            { '2', (-1, 1) },
            { '1', (-2, 1) },
            { '6', (0, 2) },
            { '5', (-1, 2) },
            { '4', (-2, 2) },
            { '9', (0, 3) },
            { '8', (-1, 3) },
            { '7', (-2, 3) }
        };
        
        Moves = new Dictionary<(char From, char To), string>
        {
            {('A', '0'), "<"},
            {('A', '3'), "^"},
            {('0', '2'), "^"},
            {('3', '2'), "<"},
            {('2', '1'), "<"},
            {('3', '6'), "^"},
            {('2', '5'), "^"},
            {('1', '4'), "^"},
            {('6', '5'), "<"},
            {('5', '4'), "<"},
            {('6', '9'), "^"},
            {('5', '8'), "^"},
            {('4', '7'), "^"},
            {('9', '8'), "<"},
            {('8', '7'), "<"},
        };
        Init();
    }
}