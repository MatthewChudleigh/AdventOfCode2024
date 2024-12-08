namespace A08.Test;

public class Test
{
    [Fact]
    public void TestLinear()
    {
        var map = new A08.Solution.Map()
        {
            Height = 10,
            Width = 10,
            Antennas =
            {
                { '0', [(4, 4), (4, 5)] }
            }
        };

        var result = map.UniqueAntinodes(false);
        Assert.Equal(2, result.Count);
        Assert.Contains((4,3), result);
        Assert.Contains((4,6), result);
    }
    
    [Fact]
    public void TestDiagonal()
    {
        var map = new A08.Solution.Map()
        {
            Height = 10,
            Width = 10,
            Antennas =
            {
                { '0', [(4, 4), (5, 5)] }
            }
        };

        var result = map.UniqueAntinodes(false);
        Assert.Equal(2, result.Count);
        Assert.Contains((3,3), result);
        Assert.Contains((6,6), result);
    }
    
    [Fact]
    public void TestOffsetDiagonal()
    {
        var map = new A08.Solution.Map()
        {
            Height = 10,
            Width = 10,
            Antennas =
            {
                { '0', [(4, 4), (5, 6)] }
            }
        };

        var result = map.UniqueAntinodes(false);
        Assert.Equal(2, result.Count);
        Assert.Contains((3,2), result);
        Assert.Contains((6,8), result);
    }
    
    [Fact]
    public void TestOutOfBounds()
    {
        var map = new A08.Solution.Map()
        {
            Height = 10,
            Width = 10,
            Antennas =
            {
                { '0', [(4, 3), (8, 4)] }
            }
        };

        var result = map.UniqueAntinodes(false);
        Assert.Single(result);
        Assert.Contains((0,2), result);
    }
}