using Xunit.Abstractions;

namespace A15.Test;

public class Test
{
    private readonly ITestOutputHelper _testOutputHelper;

    public Test(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    private const string SmallMap = """
                                    ########
                                    #..O.O.#
                                    ##@.O..#
                                    #...O..#
                                    #.#.O..#
                                    #...O..#
                                    #......#
                                    ########
                                    """;

    private const string ExpectedSmallMap1 = """
                                             ########
                                             #..O.O.#
                                             ##@.O..#
                                             #...O..#
                                             #.#.O..#
                                             #...O..#
                                             #......#
                                             ########
                                             """;

    private const string ExpectedSmallMap2 = """
                                             ########
                                             #.@O.O.#
                                             ##..O..#
                                             #...O..#
                                             #.#.O..#
                                             #...O..#
                                             #......#
                                             ########
                                             """;

    private const string ExpectedSmallMap3 = """
                                             ########
                                             #.@O.O.#
                                             ##..O..#
                                             #...O..#
                                             #.#.O..#
                                             #...O..#
                                             #......#
                                             ########
                                             """;

    private const string ExpectedSmallMap4 = """
                                             ########
                                             #..@OO.#
                                             ##..O..#
                                             #...O..#
                                             #.#.O..#
                                             #...O..#
                                             #......#
                                             ########
                                             """;

    private const string ExpectedSmallMap5 = """
                                             ########
                                             #...@OO#
                                             ##..O..#
                                             #...O..#
                                             #.#.O..#
                                             #...O..#
                                             #......#
                                             ########
                                             """;

    private const string ExpectedSmallMap6 = """
                                             ########
                                             #...@OO#
                                             ##..O..#
                                             #...O..#
                                             #.#.O..#
                                             #...O..#
                                             #......#
                                             ########
                                             """;

    private const string ExpectedSmallMap7 = """
                                             ########
                                             #....OO#
                                             ##..@..#
                                             #...O..#
                                             #.#.O..#
                                             #...O..#
                                             #...O..#
                                             ########
                                             """;

    private const string ExpectedSmallMap8 = """
                                             ########
                                             #....OO#
                                             ##..@..#
                                             #...O..#
                                             #.#.O..#
                                             #...O..#
                                             #...O..#
                                             ########
                                             """;

    private const string ExpectedSmallMap9 = """
                                             ########
                                             #....OO#
                                             ##.@...#
                                             #...O..#
                                             #.#.O..#
                                             #...O..#
                                             #...O..#
                                             ########
                                             """;

    private const string ExpectedSmallMap10 = """
                                             ########
                                             #....OO#
                                             ##.....#
                                             #..@O..#
                                             #.#.O..#
                                             #...O..#
                                             #...O..#
                                             ########
                                             """;

    private const string ExpectedSmallMap11 = """
                                              ########
                                              #....OO#
                                              ##.....#
                                              #...@O.#
                                              #.#.O..#
                                              #...O..#
                                              #...O..#
                                              ########
                                              """;

    private const string ExpectedSmallMap12 = """
                                              ########
                                              #....OO#
                                              ##.....#
                                              #....@O#
                                              #.#.O..#
                                              #...O..#
                                              #...O..#
                                              ########
                                              """;

    private const string ExpectedSmallMap13 = """
                                              ########
                                              #....OO#
                                              ##.....#
                                              #.....O#
                                              #.#.O@.#
                                              #...O..#
                                              #...O..#
                                              ########
                                              """;

    private const string ExpectedSmallMap14 = """
                                              ########
                                              #....OO#
                                              ##.....#
                                              #.....O#
                                              #.#O@..#
                                              #...O..#
                                              #...O..#
                                              ########
                                              """;

    private const string ExpectedSmallMap15 = """
                                              ########
                                              #....OO#
                                              ##.....#
                                              #.....O#
                                              #.#O@..#
                                              #...O..#
                                              #...O..#
                                              ########
                                              """;

    private const string MedMap = """
                                  ##########
                                  #..O..O.O#
                                  #......O.#
                                  #.OO..O.O#
                                  #..O@..O.#
                                  #O#..O...#
                                  #O..O..O.#
                                  #.OO.O.OO#
                                  #....O...#
                                  ##########
                                  """;
    
    private const string ExpectedMedMap = """
                                    ##########
                                    #.O.O.OOO#
                                    #........#
                                    #OO......#
                                    #OO@.....#
                                    #O#.....O#
                                    #O.....OO#
                                    #O.....OO#
                                    #OO....OO#
                                    ##########
                                    """;

    private const string MedMoves = """
                                    <vv>^<v^>v>^vv^v>v<>v^v<v<^vv<<<^><<><>>v<vvv<>^v^>^<<<><<v<<<v^vv^v>^
                                    vvv<<^>^v^^><<>>><>^<<><^vv^^<>vvv<>><^^v>^>vv<>v<<<<v<^v>^<^^>>>^<v<v
                                    ><>vv>v^v^<>><>>>><^^>vv>v<^^^>>v^v^<^^>v^^>v^<^v>v<>>v^v^<v>v^^<^^vv<
                                    <<v<^>>^^^^>>>v^<>vvv^><v<<<>^^^vv^<vvv>^>v<^^^^v<>^>vvvv><>>v^<<^^^^^
                                    ^><^><>>><>^^<<^^v>>><^<v>^<vv>>v>>>^v><>^v><<<<v>>v<v<v>vvv>^<><<>^><
                                    ^>><>^v<><^vvv<^^<><v<<<<<><^v<<<><<<^^<v<^^^><^>>^<v^><<<^>>^v<v^v<v^
                                    >^>>^v>vv>^<<^v<>><<><<v<<v><>v<^vv<<<>^^v^>^^>>><<^v>>v^v><^^>>^<>vv^
                                    <><^^>^^^<><vvvvv^v<v<<>^v<v>v<<^><<><<><<<^^<<<^<<>><<><^^^>^^<>^>v<>
                                    ^^>vv<^v^v<vv>^<><v<^v>^^^>>>^^vvv^>vvv<>>>^<^>>>>>^<<^v>^vvv<>^<><<v>
                                    v^^>>><<^^<>>^v^<v^vv<>v^<<>^<^v^v><^<<<><<^<v><v<>vv>>v><v^<vv<>v^<<^
                                    """;
    
    [Theory]
    [InlineData(SmallMap, "<", ExpectedSmallMap1, null)]
    [InlineData(ExpectedSmallMap1, "^", ExpectedSmallMap2, null)]
    [InlineData(ExpectedSmallMap2, "^", ExpectedSmallMap3, null)]
    [InlineData(ExpectedSmallMap3, ">", ExpectedSmallMap4, null)]
    [InlineData(ExpectedSmallMap4, ">", ExpectedSmallMap5, null)]
    [InlineData(ExpectedSmallMap5, ">", ExpectedSmallMap6, null)]
    [InlineData(ExpectedSmallMap6, "v", ExpectedSmallMap7, null)]
    [InlineData(ExpectedSmallMap7, "v", ExpectedSmallMap8, null)]
    [InlineData(ExpectedSmallMap8, "<", ExpectedSmallMap9, null)]
    [InlineData(ExpectedSmallMap9, "v", ExpectedSmallMap10, null)]
    [InlineData(ExpectedSmallMap10, ">", ExpectedSmallMap11, null)]
    [InlineData(ExpectedSmallMap11, ">", ExpectedSmallMap12, null)]
    [InlineData(ExpectedSmallMap12, "v", ExpectedSmallMap13, null)]
    [InlineData(ExpectedSmallMap13, "<", ExpectedSmallMap14, null)]
    [InlineData(ExpectedSmallMap14, "<", ExpectedSmallMap15, 2028)]
    [InlineData(MedMap, MedMoves, ExpectedMedMap, 10092)]
    public void TestRobotMoves(string initMap, string moves, string expectedMap, int? expectedSum)
    {
        var map = Solution.LinesToMap(initMap.Split('\n', StringSplitOptions.RemoveEmptyEntries)); 
        _testOutputHelper.WriteLine(map.Render());
        map.Apply(moves);
        var mapString = map.Render();
        _testOutputHelper.WriteLine(mapString);
        Assert.Equal(expectedMap.ReplaceLineEndings(""), mapString.ReplaceLineEndings(""));
        if (!expectedSum.HasValue) return;
        var sum = Solution.Calculate(map);
        Assert.Equal(expectedSum, sum);
    }
}