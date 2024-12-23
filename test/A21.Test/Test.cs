using FluentAssertions;
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
    [InlineData(0,"3A", "3A", 6)]
    [InlineData(1,"3A", "^AvA", 12)]
    [InlineData(1,"029A", "<A^A>^^AvvvA", 348)]
    [InlineData(2,"029A", "<v<A>>^A<A>AvA<^AA>A<vAAA>^A", 812)]
    [InlineData(3,"029A", "<vA<AA>>^AvAA<^A>A<v<A>>^AvA^A<vA>^A<v<A>^A>AAvA^A<v<A>A>^AAAvA<^A>A", 68 * 29)]
    [InlineData(3,"980A", "<v<A>>^AAAvA^A<vA<AA>>^AvAA<^A>A<v<A>A>^AAAvA<^A>A<vA>^A<A>A", 60 * 980)]
    [InlineData(3,"179A", "<v<A>>^A<vA<A>>^AAvAA<^A>A<v<A>>^AAvA^A<vA>^AA<A>A<v<A>A>^AAAvA<^A>A", 68 * 179)]
    [InlineData(3,"456A", "<v<A>>^AA<vA<A>>^AAvAA<^A>A<vA>^A<A>A<vA>^A<A>A<v<A>A>^AAvA<^A>A", 64 * 456)]    
    [InlineData(3,"379A", "<v<A>>^AvA^A<vA<AA>>^AAvA<^A>AAvA^A<vA>^AA<A>A<v<A>A>^AAAvA<^A>A", 64 * 379)]

    public void TestSolution(int robots, string code, string expectedCode, int expectedCost)
    {
        var result = Solution.Calculate(robots, code);
        _testOutputHelper.WriteLine(result.Code);
        result.Code.Should().Be(expectedCode);
        result.Cost.Should().Be(expectedCost);
    }
}