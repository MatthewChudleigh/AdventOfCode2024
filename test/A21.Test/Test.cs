using Xunit.Abstractions;

namespace A21.Test;

public class Test
{
    private readonly ITestOutputHelper _testOutputHelper;

    public Test(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    [Theory]
    [InlineData(1, 'A', "0", "<A", 0)]
    [InlineData(1, 'A', "1", "^<<A", 4)]
    [InlineData(1, '1', "7", "^^A", 21)]
    [InlineData(1, 'A', "3", "^A", 6)]
    [InlineData(1, 'A', "2", "^<A", 6)]
    [InlineData(1, 'A', "8", "^^^<A", 40)]
    [InlineData(1, '1', "0", ">vA", 0)]
    [InlineData(1, '4', "A", ">>vvA", 0)]
    [InlineData(1, 'A', "029A", "<A^A>^^AvvvA", 348)]
    [InlineData(1, 'A', "179A", "^<<A^^A>>AvvvA", 2506)]
    [InlineData(2, 'A', "0", "v<<A>>^A", 0)]
    [InlineData(2, 'A', "029A", "v<<A>>^A<A>AvA^<AA>Av<AAA>^A", 812)]
    [InlineData(3, 'A', "029A", "v<A<AA>>^AvAA^<A>Av<<A>>^AvA^Av<A>^A<Av<A>>^AAvA^Av<A<A>>^AAAvA^<A>A", 68 * 29)]
    [InlineData(3, 'A', "980A", "v<<A>>^AAAvA^Av<A<AA>>^AvAA^<A>Av<A<A>>^AAAvA^<A>Av<A>^A<A>A", 60 * 980)]
    [InlineData(3, 'A', "179A", "v<<A>>^Av<A<A>>^AAvAA^<A>Av<<A>>^AAvA^Av<A>^AA<A>Av<A<A>>^AAAvA^<A>A", 68 * 179)]
    [InlineData(3, 'A', "456A", "v<<A>>^AAv<A<A>>^AAvAA^<A>Av<A>^A<A>Av<A>^A<A>Av<A<A>>^AAvA^<A>A", 64 * 456)]
    
    [InlineData(1, 'A', "379A", "^A^^<<A>>AvvvA", 5306)]
    [InlineData(2, 'A', "379A", "<A>A<AAv<AA>>^AvAA^Av<AAA>^A", 10612)]
    [InlineData(3, 'A', "379A", "<v<A>>^AvA^A<vA<AA>>^AAvA<^A>AAvA^A<vA>^AA<A>A<v<A>A>^AAAvA<^A>A", 64 * 379)]
    public void TestSolution(int robots, char start, string code, string expectedMoves, int expectedTotal)
    {
        var (moves, total) = Solution.Calculate(start, code, robots);
        _testOutputHelper.WriteLine(moves);
        Assert.Equal(expectedTotal, total);
        Assert.Equal(expectedMoves, moves);
    }
}