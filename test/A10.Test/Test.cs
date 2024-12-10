namespace A10.Test;

public class Test
{
    [Theory]
    [InlineData(false, 1)]
    [InlineData(true, 2)]
    public void TestBasicMap(bool fork, int expectedScore)
    {
        var map = new Solution.Map()
        {
            TrailHeads = [(0, 0)],
            Points = new Dictionary<(int X, int Y), Solution.MapPoint>()
            {
                { (0, 0), new Solution.MapPoint(0) },
                { (0, 1), new Solution.MapPoint(1) },
                { (1, 0), new Solution.MapPoint(1) },
                { (1, 1), new Solution.MapPoint(2) }
            }
        };
        
        map.Targets.Push(new Solution.Target(1)
        { 
            Point = (1, 1),
            Height = 2
        });

        var score = Solution.Score(map, fork);
        Assert.Equal(expectedScore, score);
    }
}
