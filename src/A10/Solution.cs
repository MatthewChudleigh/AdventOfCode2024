using System.Collections;

namespace A10;

public static class Solution
{
    public class Target(int id)
    {
        public int Id => id;
        public int Height { get; set; }
        public (int X, int Y) Point { get; set; }
    }

    public class MapPoint(int height)
    {
        public int Height => height;
        public HashSet<int> Targets { get; set; } = new();
    }
    
    public class Map
    {
        public int NextTargetId { get; set; }
        public Dictionary<int, Target> Targets { get; set; } = new();
        public HashSet<(int X, int Y)> TrailHeads { get; set; } = new();
        public Dictionary<(int X, int Y), MapPoint> Points { get; set; } = new();
        public int Width { get; set; }
        public int Height { get; set; }

        public static Map Read(string dataPath, int start = 0, int target = 9)
        {
            var map = new Map();

            var y = 0;
            foreach (var line in File.ReadAllLines(dataPath))
            {
                var x = 0;
                foreach (var c in line)
                {
                    if (c != '.')
                    {
                        var point = (x, y);
                        var height = c - 48;
                        
                        if (height == target)
                        {
                            ++map.NextTargetId;
                            map.Points[point] = new MapPoint(height) { Targets = [map.NextTargetId] };
                            map.Targets[map.NextTargetId] = new Target(map.NextTargetId)
                            {
                                Height = height,
                                Point = point
                            };
                        }
                        else
                        {
                            map.Points[point] = new MapPoint(height);
                            if (height == start)
                            {
                                map.TrailHeads.Add(point);
                            }
                        }
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
    
    public static int Score(string dataPath, bool newIdOnFork, int start = 0, int targetHeight = 9)
    {
        var map = Map.Read(dataPath, start, targetHeight);

        return Score(map, newIdOnFork);
    }

    public static int Score(Map map, bool newIdOnFork)
    {
        var score = 0;

        var targets = new Stack<Target>(map.Targets.Values);
        while (targets.Count > 0)
        {
            var target = targets.Pop();
            var (x, y) = target.Point;

            var dirs = (new List<(int X, int Y)>()
                    { (x + 1, y + 0), (x + 0, y + 1), (x - 1, y - 0), (x - 0, y - 1) })
                .Where(xy =>
                    xy.X >= 0 && xy.X < map.Width && xy.Y >= 0 && xy.Y < map.Height &&
                    map.Points.ContainsKey(xy) && map.Points[xy].Height == target.Height - 1)
                .ToList();

            foreach (var dir in dirs)
            {
                var point = map.Points[dir];
                if (point.Targets.Add(target.Id))
                {
                    targets.Push(new Target(newIdOnFork ? ++map.NextTargetId : target.Id)
                    {
                        Point = dir,
                        Height = point.Height,
                    });
                }
            }
        }

        foreach (var start in map.TrailHeads)
        {
            score += map.Points[start].Targets.Count;
        }
        
        return score;
    }
}