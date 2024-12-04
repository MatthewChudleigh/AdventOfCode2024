var dataPath = "/workspaces/AdventOfCode2024/data/A04/A04.1.txt";

var xmas = new A04.Xmas();

var y = 0;
foreach (var line in File.ReadAllLines(dataPath))
{
    var x = 0;
    foreach (var c in line)
    {
        switch (c)
        {
            case 'X': xmas.X.Add((x, y)); break;
            case 'M': xmas.M.Add((x, y)); break;
            case 'A': xmas.A.Add((x, y)); break;
            case 'S': xmas.S.Add((x, y)); break;
        }
        x++;
    }
    y++;
}

var count = A04.XmasCount(xmas);
Console.WriteLine(count);

static class A04
{
    public class Xmas
    {
        public HashSet<(int X, int Y)> X { get; set; } = new HashSet<(int X, int Y)>();
        public HashSet<(int X, int Y)> M { get; set; } = new HashSet<(int X, int Y)>();
        public HashSet<(int X, int Y)> A { get; set; } = new HashSet<(int X, int Y)>();
        public HashSet<(int X, int Y)> S { get; set; } = new HashSet<(int X, int Y)>();
    }

    public static int XmasCount(Xmas xmas)
    {
        var count = 0;
        var points = new List<HashSet<(int X, int Y)>>() { xmas.X, xmas.M, xmas.A, xmas.S };
        foreach (var p in xmas.X)
        {
            if (check(points, p.X, p.Y, 1, 0)) count++;
            if (check(points, p.X, p.Y, 1, 1)) count++;
            if (check(points, p.X, p.Y, 0, 1)) count++;
            if (check(points, p.X, p.Y, -1, 1)) count++;
            if (check(points, p.X, p.Y, -1, 0)) count++;
            if (check(points, p.X, p.Y, -1, -1)) count++;
            if (check(points, p.X, p.Y, 0, -1)) count++;
            if (check(points, p.X, p.Y, 1, -1)) count++;
        }

        return count;
    }

    private static bool check(List<HashSet<(int X, int Y)>> points, int x, int y, int offX, int offY)
    {
        foreach (var p in points)
        {
            if (p.Contains((x, y)))
            {
                x += offX;
                y += offY;
            }
            else
            {
                return false;
            }
        }

        return true;
    }
}