namespace A08;

using Common;

public static class Solution
{
    public class Map
    {
        public int Width { get; set; }
        public int Height { get; set; }
        
        public readonly Dictionary<char, List<(int X, int Y)>> Antennas = new();
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

    public static void PrintAntiNodes(Map map, HashSet<(int X, int Y)> antinodes)
    {
        for (var j = 0; j < map.Height; j++)
        {
            for (var i = 0; i < map.Width; i++)
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

    public static HashSet<(int X, int Y)> UniqueAntinodes(Map map, bool findAll)
    {
        var antinodes = new HashSet<(int X, int Y)>();
        foreach (var antenna in map.Antennas)
        {
            for (var a = 0; a < antenna.Value.Count; a++)
            {
                if (findAll && antenna.Value.Count > 1)
                {
                    antinodes.Add(antenna.Value[a]);
                }
                for (var b = a + 1; b < antenna.Value.Count; b++)
                {
                    var aX = antenna.Value[a].X;
                    var aY = antenna.Value[a].Y;
                    var bX = antenna.Value[b].X;
                    var bY = antenna.Value[b].Y;
                    
                    var xD = bX - aX;
                    var yD = bY - aY;

                    var hasAntinode = true;
                    while (hasAntinode)
                    {
                        hasAntinode = aX - xD >= 0 && aX - xD < map.Width && aY - yD >= 0 && aY - yD < map.Height;
                        if (hasAntinode)
                        {
                            antinodes.Add((aX - xD, aY - yD));
                            aX -= xD;
                            aY -= yD;
                        }
                        
                        if (!findAll)
                        {
                            break;
                        }
                    }

                    hasAntinode = true;
                    while (hasAntinode)
                    {
                        hasAntinode = bX + xD >= 0 && bX + xD < map.Width && bY + yD >= 0 && bY + yD < map.Height;
                        if (hasAntinode)
                        {
                            antinodes.Add((bX + xD, bY + yD));
                            bX += xD;
                            bY += yD;
                        }
                        
                        if (!findAll)
                        {
                            break;
                        }
                    }
                }
            }
        }
        return antinodes;
    }
}