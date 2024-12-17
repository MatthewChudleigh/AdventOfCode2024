namespace A16.Test;

public class Test
{
    public const string SmallMap1 = """
                                   S.E
                                   """;
    
    public const string SmallMap2 = """
                                   ...
                                   S#E
                                   ...
                                   """;
    
    public const string SmallMap3 = """
                                    .#.
                                    S#E
                                    .#.
                                    """;

    public const string MedMap1 = """
                                  ###############
                                  #.......#....E#
                                  #.#.###.#.###.#
                                  #.....#.#...#.#
                                  #.###.#####.#.#
                                  #.#.#.......#.#
                                  #.#.#####.###.#
                                  #...........#.#
                                  ###.#.#####.#.#
                                  #...#.....#.#.#
                                  #.#.#.###.#.#.#
                                  #.....#...#.#.#
                                  #.###.#.#.#.#.#
                                  #S..#.....#...#
                                  ###############
                                  """;
    
    public const string MedMap2 = """
                                  #################
                                  #...#...#...#..E#
                                  #.#.#.#.#.#.#.#.#
                                  #.#.#.#...#...#.#
                                  #.#.#.#.###.#.#.#
                                  #...#.#.#.....#.#
                                  #.#.#.#.#.#####.#
                                  #.#...#.#.#.....#
                                  #.#.#####.#.###.#
                                  #.#.#.......#...#
                                  #.#.###.#####.###
                                  #.#.#...#.....#.#
                                  #.#.#.#####.###.#
                                  #.#.#.........#.#
                                  #.#.#.#########.#
                                  #S#.............#
                                  #################
                                  """;

    [Theory]
    [InlineData(SmallMap1, 2)]
    [InlineData(SmallMap2, 3004)]
    [InlineData(SmallMap3, null)]
    [InlineData(MedMap1, 7036)]
    [InlineData(MedMap2, 11048)]
    public void MapTest(string mapString, int? expectedMinScore)
    {
        var lines = mapString.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        var map = Solution.LinesToMap(lines);
        var minScore = Solution.CalculateMinScore(map);
        Assert.Equal(expectedMinScore, minScore);
    }
}
