namespace A19.Test;

public class Test
{
    private const string Small = """
                                 r, wr, b, g, bwu, rb, gb, br

                                 brwrr
                                 bggr
                                 gbbr
                                 rrbgbr
                                 ubwu
                                 bwurrg
                                 brgr
                                 bbrgwb
                                 """;
    
    [Theory]
    [InlineData(Small)]
    public void TestPattern(string input)
    {
        var onsen = Solution.Load(input.Split('\n'));
        Assert.Equal(8, onsen.Towels.Count);
        Assert.Equal(8, onsen.Designs.ToList().Count);

        var possible = Solution.PossibleDesigns(onsen);
        Assert.Equal(6, possible);
    }

    [Theory]
    [InlineData("r", true)]
    [InlineData("wr", true)]
    [InlineData("b", true)]
    [InlineData("g", true)]
    [InlineData("bwu", true)]
    [InlineData("rb", true)]
    [InlineData("gb", true)]
    [InlineData("br", true)]
    [InlineData("brwrr", true)]
    [InlineData("bggr", true)]
    [InlineData("gbbr", true)]
    [InlineData("rrbgbr", true)]
    [InlineData("ubwu", false)]
    [InlineData("bwurrg", true)]
    [InlineData("brgr", true)]
    [InlineData("bbrgwb", false)]
    public void TestTowel(string design, bool expectedPossible)
    {
        var onsen = new Solution.Onsen("r, wr, b, g, bwu, rb, gb, br", []);
        var isPossible = onsen.IsPossible([design]);
        Assert.Equal(expectedPossible, isPossible);
    }
}