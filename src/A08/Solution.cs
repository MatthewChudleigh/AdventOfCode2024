namespace A08;

using Common;

public static class Solution
{
    public class Map
    {
        public int Width { get; set; }
        public int Height { get; set; }
        
        public readonly Dictionary<char, List<(int X, int Y)>> Antennas = new();
        

        public HashSet<(int X, int Y)> UniqueAntinodes(bool findAll)
        {
            var antinodes = new HashSet<(int X, int Y)>();
            foreach (var antinode in Antennas.SelectMany(a => FindAntinodesForAntennas(findAll, a.Value)))
            {
               antinodes.Add(antinode);
            }
            return antinodes;
        }

        private IEnumerable<(int X, int Y)> FindAntinodesForAntennas(bool findAll, List<(int X, int Y)> antenna)
        {
            for (var a = 0; a < antenna.Count; a++)
            {
                if (findAll && antenna.Count > 1)
                {
                    yield return antenna[a];
                }
                for (var b = a + 1; b < antenna.Count; b++)
                {
                    var axy = antenna[a];
                    var bxy = antenna[b];

                    foreach (var antinode in FindAntinodesForAntenna(findAll, axy, bxy))
                    {
                        yield return antinode;
                    }
                }
            }
        }

        private IEnumerable<(int X, int Y)> FindAntinodesForAntenna(bool findAll, (int X, int Y) axy, (int X, int Y) bxy)
        {
            var xD = bxy.X - axy.X;
            var yD = bxy.Y - axy.Y;

            foreach (var antinode in FindAntinodes(axy, xD * -1, yD * -1))
            {
                yield return antinode;
                if (!findAll) break;
            }
                        
            foreach (var antinode in FindAntinodes(bxy, xD, yD))
            {
                yield return antinode;
                if (!findAll) break;
            }
        }

        public IEnumerable<(int X, int Y)> FindAntinodes((int X, int Y) xy, int dx, int dy)
        {
            while (true)
            {
                var hasAntinode = xy.X + dx >= 0 && xy.X + dx < Width && xy.Y + dy >= 0 && xy.Y + dy < Height;
                if (!hasAntinode) yield break;
                
                xy.X += dx;
                xy.Y += dy;
                yield return (xy.X, xy.Y);
            }
        }
        
        public void PrintAntiNodes(HashSet<(int X, int Y)> antinodes)
        {
            for (var j = 0; j < Height; j++)
            {
                for (var i = 0; i < Width; i++)
                {
                    if (antinodes.Contains((i, j)))
                    {
                        Console.Write('#');
                    }
                    else
                    {
                        Console.Write('.');
                    }
                }
                Console.WriteLine();
            }
        }
    }

    public static Map ReadMap(string dataPath)
    {
        var map = new Map();
        var y = 0;
        foreach (var line in File.ReadLines(dataPath))
        {
            var x = 0;
            foreach (var cell in line)
            {
                if (cell != '.' && cell != '#')
                {
                    map.Antennas.AddToSet(cell, (x, y));
                }

                x++;
            }

            map.Width = x;
            y++;
        }
        map.Height = y;
        
        return map;
    }

}