using System.Text;
using System.Text.RegularExpressions;

namespace A14;

public static class Solution
{
    public class Robot
    {
        public (int X, int Y) Position { get; set; }
        public (int X, int Y) Velocity { get; set; }
    }
    
    public static int Solve(IEnumerable<string> input, int iter, int width, int height)
    {
        var robots = Parse(input).ToList();
        return Solve(robots, iter, width, height);
    }

    public static int Solve(ICollection<Robot> robots, int iter, int width, int height)
    {
        var qW = width / 2;
        var qH = height / 2;
        
        var quadrants = new Dictionary<(int X, int Y), int>()
        {
            {(0,0), 0},
            {(0,1), 0},
            {(1,0), 0},
            {(1,1), 0},
        };

        var lineConnect = new List<(int X, int Y)>()
        {
            (1,0),
            (-1,0),
            (0,1),
            (0,-1)
        };
        
        var lineIds = 0;
        var lineMap = new Dictionary<(int X, int Y), int>();
        var lines = new Dictionary<int, HashSet<(int X, int Y)>>();
        decimal avgAvgX = 0.0m, avgAvgY = 0.0m;
        
        for (var i = 0; i < iter; i++)
        {
            decimal avgX = 0.0m, avgY = 0.0m;
            lineMap.Clear();
            lines.Clear();
            lineIds = 0;
            
            foreach (var robot in robots)
            {
                var (x, y) = robot.Position;
                var (vx, vy) = robot.Velocity;
                var x1 = (x + vx * 1) % width;
                var y1 = (y + vy * 1) % height;

                if (x1 < 0)
                {
                    x1 = width + x1;
                }

                if (y1 < 0)
                {
                    y1 = height + y1;
                }

                avgX += Math.Abs(x1-qW);
                avgY += Math.Abs(y1-qH);

                robot.Position = (x1, y1);

                var found = false;
                foreach (var lc in lineConnect)
                {
                    if (lineMap.TryGetValue((x1 + lc.X, y1 + lc.Y), out var lineId))
                    {
                        lineMap[robot.Position] = lineId;
                        lines[lineId].Add(robot.Position);
                        found = true;
                    }
                }

                if (!found)
                {
                    lineIds++;
                    lines[lineIds] = [robot.Position];
                    lineMap[robot.Position] = lineIds;
                }
            }
            
            avgX /= robots.Count;
            avgY /= robots.Count;
            var longLines = lines.Count(l => l.Value.Count > 4);
            if (longLines >= 55)
            {
                var grid = robots.GroupBy(r => r.Position).ToDictionary(g => g.Key, g => g.Count());
                Print(i, grid, width, height);
                Console.WriteLine($"{avgX}, {avgY}");
                Console.WriteLine($"{avgAvgX/(iter*1.0m)}, {avgAvgY/(iter*1.0m)}");
            }
            else
            {
                avgAvgX += avgX;
                avgAvgY += avgY;
            }
        }

        foreach (var robot in robots)
        {
            var (x1, y1) = robot.Position;
            var qX = (x1 < qW ? 0 : x1 > qW ? 1 : -1);
            var qY = (y1 < qH ? 0 : y1 > qH ? 1 : -1);
            if (quadrants.TryGetValue((qX, qY), out var quadrant))
            {
                quadrants[(qX, qY)] = quadrant + 1;
            }
        }

        return quadrants.Values.Aggregate(1, (acc, q) => acc * q);
    }

    public static void Print(int iter, Dictionary<(int X, int Y), int> grid, int width, int height)
    {
        var buffer = new StringBuilder();
        buffer.AppendLine($"{iter}:");

        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                if (grid.TryGetValue((x, y), out var robotCount))
                {
                    buffer.Append($"{robotCount}");
                }
                else
                {
                    buffer.Append($" ");
                }
            }

            buffer.AppendLine();
        }
        Console.Clear();
        Console.SetCursorPosition(0, 0);
        Console.Write(buffer.ToString());
        Thread.Sleep(100);
    }

    public static IEnumerable<Robot> Parse(IEnumerable<string> input)
    {
        return input.Select(line => Re.ReRobot().Match(line).Groups).Select(re => 
            new Robot()
        {
            Position = (int.Parse(re["xPos"].Value), int.Parse(re["yPos"].Value)),
            Velocity = (int.Parse(re["xVel"].Value), int.Parse(re["yVel"].Value)),
        });
    }
}

public partial class Re
{
    [GeneratedRegex(@"p=(?<xPos>[-]?\d+),(?<yPos>[-]?\d+) v=(?<xVel>[-]?\d+),(?<yVel>[-]?\d+)", RegexOptions.Compiled)]
    public static partial Regex ReRobot();
}