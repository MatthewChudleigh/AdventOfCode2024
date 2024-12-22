using System.Text;

namespace A21;

public static class Solution
{
    public static Keypad Keypad = new Keypad();
    public static Arrows Arrows = new Arrows();
    
    public static (string moves, long total) Calculate(char start, string code, int robots)
    {
        var moves = GetAllMoves(Keypad, start, (0,0), code, robots-1);
        long.TryParse(code[..^1], out var val);
        return (moves.moves, total: moves.total * val);
    }

    public static List<(int X, int Y)> Dirs { get; } =
    [
        (-1, 0), (1, 0), (0, -1), (0, 1)
    ];

    public record Step((int X, int Y) Pos, (int X, int Y) StartDir, (int X, int Y) Dir, string Moves);
    public class StepQueue : PriorityQueue<Step, int>;
    
    public static (string moves, long total) GetAllMoves(InputDevice input, char start, (int X, int Y) startDir, string code, int robots)
    {
        var total = 0L;

        var pos = input.Keys[start];
        var allMoves = new StringBuilder();
        foreach (var target in code)
        {
            var targetPos = input.Keys[target];

            long? minCache = null;
            var steps = new StepQueue();
            foreach (var dir in Dirs)
            {
                if (Costs.TryGetValue((pos, startDir, targetPos, robots), out var cache))
                {   // Going from this pos from this dir to this point at this level
                    minCache = cache <= (minCache ?? cache) ? cache : minCache;
                }
                else
                {
                    steps.Enqueue(new Step(pos, dir, dir, ""), 0);
                }
            }

            if (minCache.HasValue)
            {
                total += minCache.Value;
                pos = targetPos;
                continue;
            }

            var (moves, best) = FindBest(input, steps, target, robots);
            if (!best.HasValue) throw new Exception();
            
            allMoves.Append(moves);
            total += best.Value;
            
            pos = targetPos;
        }
        
        return (allMoves.ToString(), total);
    }

    public static (string Moves, long? Best) FindBest(InputDevice input, StepQueue steps, char end, int robots)
    {
        var minMoves = "";
        long? min = null;
        var visited = new HashSet<(int X, int Y)>();
            
        while (steps.TryDequeue(out var cur, out var dist))
        {
            var next = (X: cur.Pos.X+cur.Dir.X, Y: cur.Pos.Y+cur.Dir.Y);
            if (!input.Map.TryGetValue(next, out var nextKey)) continue;
            if (!visited.Add(next)) continue;
            
            var nextMove = input.Moves[(input.Map[cur.Pos], nextKey)];
            var moves = cur.Moves + nextMove;
                
            if (next == input.Keys[end])
            {
                moves += 'A';
                if (robots == 0)
                {
                    if (moves.Length <= (min ?? moves.Length))
                    {
                        min = moves.Length;
                        minMoves = moves;
                    }
                    
                }
                else
                {
                    var (m, cost) = GetAllMoves(Arrows, 'A', cur.StartDir, moves, robots - 1);
                    if (cost < (min ?? cost))
                    {
                        min = cost;
                        minMoves = m;
                    }
                }
            }
            else
            {
                foreach (var dir in Dirs)
                {
                    steps.Enqueue(new Step(next, cur.StartDir, dir, moves), dist + 1);
                }
            }
        }

        return (minMoves, min);
    }

    public static Dictionary<((int X, int Y) Pos, (int X, int Y) Dir, (int X, int Y) Tgt, int Robot), long> Costs = new();
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