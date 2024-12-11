namespace A11.Test;

public class Test
{
    [Fact]
    public void TestZero()
    {
        var stones = Solution.Generate([new Solution.Stone(){ Count = 1, Value = 0 }], 1);
        Assert.Single(stones);
        Assert.Equal(1, stones[0].Count);
        Assert.Equal(1, stones[0].Value);
    }
    
    [Fact]
    public void TestOne()
    {
        var stones = Solution.Generate([new Solution.Stone(){ Count = 1, Value = 1 }], 1);
        Assert.Single(stones);
        Assert.Equal(1, stones[0].Count);
        Assert.Equal(2024, stones[0].Value);
    }
    
    [Fact]
    public void TestEvenSize()
    {
        var stones = Solution.Generate([new Solution.Stone(){ Count = 1, Value = 11 }], 1);
        Assert.Single(stones);
        Assert.Equal(2, stones[0].Count);
        Assert.Equal(1, stones[0].Value);
    }
    
    [Fact]
    public void TestEvenSizeIter2()
    {
        var stones = Solution.Generate([new Solution.Stone(){ Count = 1, Value = 11 }], 2);
        Assert.Single(stones);
        Assert.Equal(2, stones[0].Count);
        Assert.Equal(2024, stones[0].Value);
    }
    
    [Fact]
    public void TestIters()
    {
        var initialSet = new List<Solution.Stone>
            { new Solution.Stone() { Count = 1, Value = 125 }, new Solution.Stone() { Count = 1, Value = 17 } };
        var stones = Solution.Generate(initialSet, 1);
        Assert.Equal(3, stones.Count);
        Assert.Equal(253000, stones[0].Value);
        Assert.Equal(1, stones[1].Value);
        Assert.Equal(7, stones[2].Value);

        stones = Solution.Generate(stones, 1);
        Assert.Equal(4, stones.Count);
        Assert.Equal(253, stones[0].Value);
        Assert.Equal(0, stones[1].Value);
        Assert.Equal(2024, stones[2].Value);
        Assert.Equal(14168, stones[3].Value);

        stones = Solution.Generate(stones, 1);
        Assert.Equal(5, stones.Count);
        Assert.Equal(512072, stones[0].Value);
        Assert.Equal(1, stones[1].Value);
        Assert.Equal(20, stones[2].Value);
        Assert.Equal(24, stones[3].Value);
        Assert.Equal(28676032, stones[4].Value);

        var stoneCount = Solution.Generate(initialSet, 6).Sum(x => x.Count);
        Assert.Equal(22, stoneCount);

        stoneCount = Solution.Generate(initialSet, 25).Sum(x => x.Count);
        Assert.Equal(55312, stoneCount);
    }
}
