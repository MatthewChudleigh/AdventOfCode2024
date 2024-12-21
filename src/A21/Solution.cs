using System.Text;

namespace A21;

public static class Solution
{
    public class Keypad
    {
        public Dictionary<char, (int X, int Y)> Keys = new()
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

        public Dictionary<(int X, int Y), char> Map { get; } = new();
        
        public Dictionary<(char From, char To), string> Moves { get; } = new() 
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
            {('4', '7'), "<"},
            {('9', '8'), "<"},
            {('8', '7'), "<"},
        };

        public void Init()
        {
            foreach (var move in Moves.ToList())
            {
                Moves[(move.Key.To, move.Key.From)] = move.Value switch
                {
                    "<" => ">",
                    "^" => "v",
                    _ => move.Value
                };
            }
            
            foreach (var key in Keys)
            {
                Map[key.Value] = key.Key;
            }
        }

        public void GetMoves(StringBuilder sb, char from, char to)
        {
            var c1 = from;
            var pos = Keys[from];
            var end = Keys[to];
            
            while (pos != end)
            {
                var dx = pos.X < end.X ? 1 : pos.X > end.X ? -1 : 0;
                var dy = pos.Y < end.Y ? 1 : pos.Y > end.Y ? -1 : 0;

                var next = (X: pos.X + dx, Y: pos.Y);
                if (dx == 0 || !Map.ContainsKey(next))
                {
                    next = (X: pos.X, Y: pos.Y + dy);
                }
                var c2 = Map[next];
                var move = Moves[(c1, c2)];
                c1 = c2;
                sb.Append(move);
                pos = next;
            }
        }
        

        public string GetMoves(string code)
        {
            var moves = new StringBuilder();
            var pos = code[0];
            foreach (var c in code.Skip(1))
            {
                GetMoves(moves, pos, c);
                pos = c;
            }

            return moves.ToString();
        }
    }
}