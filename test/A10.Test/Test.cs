namespace A10.Test;

public class Test
{
    [Fact]
    public void Test1()
    {
        var map = new Solution.Map()
        {
            Height = 2,
            Width = 2,
            TrailHeads = new HashSet<(int X, int Y)>() { (0,0) },
            Points = new Dictionary<(int X, int Y), Solution.MapPoint>()
            {
                { (0, 0), new Solution.MapPoint(0) },
                { (0, 1), new Solution.MapPoint(1) },
                { (1, 0), new Solution.MapPoint(1) },
                { (1, 1), new Solution.MapPoint(2) }
            },
            Targets = new Dictionary<int, Solution.Target>()
            {
                { 1, new Solution.Target(1)
                { 
                    Point = (1, 1),
                    Height = 2
                }}
            }
        };

        var score = Solution.Score(map);
        Assert.Equal(1, score);
    }
}
