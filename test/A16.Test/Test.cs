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
    [InlineData(SmallMap1, 2, 3)]
    [InlineData(SmallMap2, 3004, 8)]
    [InlineData(SmallMap3, null, 0)] 
    [InlineData(MedMap1, 7036, 45)]
    [InlineData(MedMap2, 11048, 64)]
    public void MapTest(string mapString, int? expectedMinScore, int expectedBestSeats)
    {
        var lines = mapString.Split('\n', StringSplitOptions.RemoveEmptyEntries);
        var map = Solution.LinesToMap(lines);
        var (minScore, bestSeats) = Solution.CalculateMinScore(map);
        Assert.Equal(expectedMinScore, minScore);
        Assert.Equal(expectedBestSeats, bestSeats);
    }
}
