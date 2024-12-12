using Xunit.Abstractions;

namespace A12.Test;

public class Test
{
    private readonly ITestOutputHelper _testOutputHelper;

    public Test(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    const string SmallGarden = """
                               AAAA
                               BBCD
                               BBCC
                               EEEC
                               """;

    private const string MedGarden = """
                                     OOOOO
                                     OXOXO
                                     OOOOO
                                     OXOXO
                                     OOOOO
                                     """;

    private const string LargeGarden = """
                                       RRRRIICCFF
                                       RRRRIICCCF
                                       VVRRRCCFFF
                                       VVRCCCJFFF
                                       VVVVCJJCFE
                                       VVIVCCJJEE
                                       VVIIICJJEE
                                       MIIIIIJJEE
                                       MIIISIJEEE
                                       MMMISSJEEE
                                       """;
    
    [Theory]
    [InlineData(SmallGarden, 5, 140)]
    [InlineData(MedGarden, 5, 772)]
    [InlineData(LargeGarden, 11, 1930)]
    public void TestGardens(string data, int expectedRegions, int expectedCost)
    {
        var garden = Solution.ToGarden(data);
        var regions = Solution.GetRegions(garden);
        var cost = Solution.CalculateCost(regions);
        Print(garden);
        Assert.Equal(expectedCost, cost);
        Assert.Equal(expectedRegions, regions.Count);
    }

    private void Print(Solution.Garden garden)
    {
        for (var h = 0; h < garden.Height; ++h)
        {
            var r = new List<int>();
            for (var w = 0; w < garden.Width; ++w)
            {
                r.Add(garden.Plots[(w, h)].Region);
            }
            _testOutputHelper.WriteLine(string.Join(" ", r.Select(x => $"{x:D2}")));
        }
    }
}