namespace A10;

public static class Solution
{
    public class Target(int id)
    {
        public int Id => id;
        public int Height { get; init; }
        public (int X, int Y) Point { get; init; }
    }

    public class MapPoint(int height)
    {
        public int Height => height;
        public HashSet<int> Targets { get; } = [];
    }
    
    public class Map
    {
        private int _nextTargetId;
        public bool NewIdOnFork { get; init; }
        public Stack<Target> Targets { get; } = new();
        public HashSet<(int X, int Y)> TrailHeads { get; init; } = [];
        public Dictionary<(int X, int Y), MapPoint> Points { get; init; } = new();

        public static Map Read(string dataPath, bool newIdOnFork, int start = 0, int target = 9)
        {
            var map = new Map()
            {
                NewIdOnFork = newIdOnFork
            };

            foreach (var (x, y, c) in File.ReadAllLines(dataPath)
                         .SelectMany((line, y) => line.Select((c, x) => (x, y, c))))
            {
                if (c == '.') continue;
                
                var point = (x, y);
                var height = c - 48;

                var mapPoint = new MapPoint(height);
                map.Points[point] = mapPoint;
                
                if (height == start)
                {
                    map.TrailHeads.Add(point);
                }
                else if (height == target)
                {
                    ++map._nextTargetId;
                    mapPoint.Targets.Add(map._nextTargetId);
                    map.Targets.Push(new Target(map._nextTargetId)
                    {
                        Height = height,
                        Point = point
                    });
                }
            }

            return map;
        }

        public int Score()
        {
            while (Targets.Count > 0)
            {
                var target = Targets.Pop();
                foreach (var t in Process(target))
                {
                    Targets.Push(t);
                }
            }

            return TrailHeads.Sum(start => Points[start].Targets.Count);
        }
        
        private IEnumerable<Target> Process(Target target)
        {
            var (x, y) = target.Point;

            var dirs = (new List<(int X, int Y)>()
                    { (x + 1, y + 0), (x + 0, y + 1), (x - 1, y - 0), (x - 0, y - 1) })
                .Where(xy => Points.ContainsKey(xy) && Points[xy].Height == target.Height - 1);

            foreach (var dir in dirs)
            {
                var point = Points[dir];
                if (!point.Targets.Add(target.Id)) continue;
                
                yield return new Target(NewIdOnFork ? ++_nextTargetId : target.Id)
                {
                    Point = dir,
                    Height = point.Height,
                };
            }
        }
    }
    
    public static int Solve(string dataPath, bool newIdOnFork, int start = 0, int targetHeight = 9)
    {
        var map = Map.Read(dataPath, newIdOnFork, start, targetHeight);
        return map.Score();
    }
}