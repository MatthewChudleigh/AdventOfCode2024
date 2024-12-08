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
                if (cell != '.')
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

    public static HashSet<(int X, int Y)> UniqueAntinodes(Map map)
    {
        var antinodes = new HashSet<(int X, int Y)>();
        foreach (var antenna in map.Antennas)
        {
            for (var a = 0; a < antenna.Value.Count; a++)
            {
                for (var b = a + 1; b < antenna.Value.Count; b++)
                {
                    var aX = antenna.Value[a].X;
                    var aY = antenna.Value[a].Y;
                    var bX = antenna.Value[b].X;
                    var bY = antenna.Value[b].Y;
                    
                    var xD = bX - aX;
                    var yD = bY - aY;

                    if (aX - xD >= 0 && aX - xD < map.Width && aY - yD >= 0 && aY - yD < map.Height)
                    {
                        antinodes.Add((aX - xD, aY - yD));
                    }
                    
                    if (bX + xD >= 0 && bX + xD < map.Width && bY + yD >= 0 && bY + yD < map.Height)
                    {
                        antinodes.Add((bX + xD, bY + yD));
                    }
                }
            }
        }
        return antinodes;
    }
}