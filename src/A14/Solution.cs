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

        foreach (var robot in robots)
        {
            var (x, y) = robot.Position;
            var (vx, vy) = robot.Velocity;
            var x1 = (x + vx * iter) % width;
            var y1 = (y + vy * iter) % height;
            
            if (x1 < 0) { x1 = width + x1; }
            if (y1 < 0) { y1 = height + y1; }
            robot.Position = (x1, y1);
            
            var qX = (x1 < qW ? 0 : x1 > qW ? 1 : -1);
            var qY = (y1 < qH ? 0 : y1 > qH ? 1 : -1);
            if (quadrants.TryGetValue((qX, qY), out var quadrant))
            {
                quadrants[(qX, qY)] = quadrant + 1;
            }
        }
        
        return quadrants.Values.Aggregate(1, (acc, q) => acc * q);
    }

    public static void Print(IEnumerable<Robot> robots, int width, int height)
    {
        var grid = robots.GroupBy(r => r.Position).ToDictionary(g => g.Key, g => g.Count());

        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                if (grid.TryGetValue((x, y), out var robotCount))
                {
                    Console.Write($"{robotCount}");
                }
                else
                {
                    Console.Write($".");
                }
            }
            Console.WriteLine();
        }
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