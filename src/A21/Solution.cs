using System.Text;

namespace A21;

public static class Solution
{
    public static Keypad Keypad { get; } = new();
    public static Directions Directions { get; } = new();

    public static (long Cost, string Code) Calculate(int robots, string code, bool getFullCode = true)
    {
        var result = Calculate(Keypad, robots, code, getFullCode);
        long.TryParse(code[..^1], out var c);
        return (c * result.Cost, result.Code);
    }

    public static (long Cost, string Code) Calculate(Input input, int robots, string code, bool getFullCode)
    {
        if (robots == 0) return (code.Length, code);

        var start = 'A';
        var totalCost = 0L;
        var fullCode = new StringBuilder();
        
        foreach (var end in code)
        {
            if (!Cache.TryGetValue((robots, start, end), out var cost))
            {
                cost = GenerateChoices(input, start, end)
                    .Select(c => Calculate(Directions, robots - 1, String.Join("",c), getFullCode))
                    .MinBy(c => c.Cost);
                
                Cache[(robots, start, end)] = cost;
            }
            
            totalCost += cost.Cost;
            if (getFullCode)
            {
                fullCode.Append(cost.Code);
            }
            
            start = end;
        }
        
        return (totalCost, fullCode.ToString());
    }
    
    public readonly static Dictionary<(int robot, char start, char end), (long Cost, string Code)> Cache = new();

    public static IEnumerable<Stack<char>> GenerateChoices(Input input, char start, char end)
    {
        if (start == end)
        {
            var m = new Stack<char>();
            m.Push('A');
            yield return m;
        }
        else
        {
            foreach (var n in input.Next(start, end))
            {
                foreach (var m in GenerateChoices(input, n.Next, end))
                {
                    m.Push(n.Move);
                    yield return m;
                }
            }
        }
    }
}

public class Input
{
    public Dictionary<char, (int X, int Y)> Points { get; set; } = new();
    public Dictionary<(int X, int Y), char> Map { get; set; } = new();
    
    public IEnumerable<(char Next, char Move)> Next(char start, char end)
    {
        var s = Points[start];
        var e = Points[end];
        var dx = (X: s.X > e.X ? -1 : s.X < e.X ? 1 : 0, Y: 0);
        var dy = (X: 0, Y: s.Y > e.Y ? -1 : s.Y < e.Y ? 1 : 0);
        if (dx.X != 0 && Map.TryGetValue((s.X + dx.X, s.Y + dx.Y), out var n))
        {
            yield return (n, Move[dx]);
        }

        if (dy.Y != 0 && Map.TryGetValue((s.X + dy.X, s.Y + dy.Y), out n))
        {
            yield return (n, Move[dy]);
        }
    }

    public static readonly Dictionary<(int X, int Y), char> Move = new()
    {
        {(-1, 0), '<'},
        {(1,0), '>'},
        {(0,-1), 'v'},
        {(0,1), '^'}
    };
}

public class Keypad : Input
{
    /*
+---+---+---+
| 7 | 8 | 9 |
+---+---+---+
| 4 | 5 | 6 |
+---+---+---+
| 1 | 2 | 3 |
+---+---+---+
    | 0 | A |
    +---+---+
 */
    public Keypad()
    {
        Points = new Dictionary<char, (int X, int Y)>
        {
            { 'A', (0,0) },
            { '0', (-1,0) },
            { '1', (-2,1) },
            { '2', (-1,1) },
            { '3', (0,1) },
            { '4', (-2,2) },
            { '5', (-1,2) },
            { '6', (0,2) },
            { '7', (-2,3) },
            { '8', (-1,3) },
            { '9', (0,3) },
        };
    
        Map = 
            Points.ToDictionary(p => p.Value, p => p.Key);
    }
}

public class Directions : Input
{ 
    /*
    +---+---+
    | ^ | A |
+---+---+---+
| < | v | > |
+---+---+---+
*/
    public Directions()
    {
        Points = new Dictionary<char, (int X, int Y)>
        {
            { 'A', (0,0) },
            { '^', (-1,0) },
            { '<', (-2,-1) },
            { 'v', (-1,-1) },
            { '>', (0,-1) },
        };

        Map =
            Points.ToDictionary(p => p.Value, p => p.Key);
    }

}