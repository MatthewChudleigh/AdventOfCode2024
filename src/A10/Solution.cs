namespace A10;

public static class Solution
{
    public class Step(int id)
    {
        public int Id => id;
        public int Height { get; init; }
        public (int X, int Y) Point { get; init; }
    }

    public class MapPoint((int X, int Y) point, int height)
    {
        public (int X, int Y) Point => point;
        public int Height => height;
        public HashSet<int> Steps { get; } = [];
    }
    
    public class Map
    {
        private int _nextPathId;
        public bool NewIdOnFork { get; init; }
        public Stack<Step> Steps { get; } = new();
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

                var mapPoint = new MapPoint(point, height);
                map.Points[point] = mapPoint;
                
                if (height == start)
                {
                    map.TrailHeads.Add(point);
                }
                else if (height == target)
                {
                    ++map._nextPathId;
                    mapPoint.Steps.Add(map._nextPathId);
                    map.Steps.Push(new Step(map._nextPathId)
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
            while (Steps.Count > 0)
            {
                foreach (var t in NextSteps(Steps.Pop()))
                {
                    Steps.Push(t);
                }
            }

            return TrailHeads.Sum(start => Points[start].Steps.Count);
        }

        private static readonly List<(int X, int Y)> CardinalPoints = [(1, 0), (0, 1), (-1, 0), (0, -1)];
        
        private IEnumerable<Step> NextSteps(Step step)
        {
            return CardinalPoints
                .Select(p => (step.Point.X + p.X, step.Point.Y + p.Y))
                .Where(xy => Points.ContainsKey(xy) && Points[xy].Height == step.Height - 1)
                .Select(xy => Points[xy])
                .Where(point => point.Steps.Add(step.Id))
                .Select(point => new Step(NewIdOnFork ? ++_nextPathId : step.Id)
                {
                    Point = point.Point,
                    Height = point.Height,
                });
        }
    }
    
    public static int Solve(string dataPath, bool newIdOnFork, int start = 0, int targetHeight = 9)
    {
        var map = Map.Read(dataPath, newIdOnFork, start, targetHeight);
        return map.Score();
    }
}