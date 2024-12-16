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

    private const string SmallMap1 = """
                                             ########
                                             #..O.O.#
                                             ##@.O..#
                                             #...O..#
                                             #.#.O..#
                                             #...O..#
                                             #......#
                                             ########
                                             """;

    private const string SmallMap2 = """
                                             ########
                                             #.@O.O.#
                                             ##..O..#
                                             #...O..#
                                             #.#.O..#
                                             #...O..#
                                             #......#
                                             ########
                                             """;

    private const string SmallMap3 = """
                                             ########
                                             #.@O.O.#
                                             ##..O..#
                                             #...O..#
                                             #.#.O..#
                                             #...O..#
                                             #......#
                                             ########
                                             """;

    private const string SmallMap4 = """
                                             ########
                                             #..@OO.#
                                             ##..O..#
                                             #...O..#
                                             #.#.O..#
                                             #...O..#
                                             #......#
                                             ########
                                             """;

    private const string SmallMap5 = """
                                             ########
                                             #...@OO#
                                             ##..O..#
                                             #...O..#
                                             #.#.O..#
                                             #...O..#
                                             #......#
                                             ########
                                             """;

    private const string SmallMap6 = """
                                             ########
                                             #...@OO#
                                             ##..O..#
                                             #...O..#
                                             #.#.O..#
                                             #...O..#
                                             #......#
                                             ########
                                             """;

    private const string SmallMap7 = """
                                             ########
                                             #....OO#
                                             ##..@..#
                                             #...O..#
                                             #.#.O..#
                                             #...O..#
                                             #...O..#
                                             ########
                                             """;

    private const string SmallMap8 = """
                                             ########
                                             #....OO#
                                             ##..@..#
                                             #...O..#
                                             #.#.O..#
                                             #...O..#
                                             #...O..#
                                             ########
                                             """;

    private const string SmallMap9 = """
                                             ########
                                             #....OO#
                                             ##.@...#
                                             #...O..#
                                             #.#.O..#
                                             #...O..#
                                             #...O..#
                                             ########
                                             """;

    private const string SmallMap10 = """
                                             ########
                                             #....OO#
                                             ##.....#
                                             #..@O..#
                                             #.#.O..#
                                             #...O..#
                                             #...O..#
                                             ########
                                             """;

    private const string SmallMap11 = """
                                              ########
                                              #....OO#
                                              ##.....#
                                              #...@O.#
                                              #.#.O..#
                                              #...O..#
                                              #...O..#
                                              ########
                                              """;

    private const string SmallMap12 = """
                                              ########
                                              #....OO#
                                              ##.....#
                                              #....@O#
                                              #.#.O..#
                                              #...O..#
                                              #...O..#
                                              ########
                                              """;

    private const string SmallMap13 = """
                                              ########
                                              #....OO#
                                              ##.....#
                                              #.....O#
                                              #.#.O@.#
                                              #...O..#
                                              #...O..#
                                              ########
                                              """;

    private const string SmallMap14 = """
                                              ########
                                              #....OO#
                                              ##.....#
                                              #.....O#
                                              #.#O@..#
                                              #...O..#
                                              #...O..#
                                              ########
                                              """;

    private const string SmallMap15 = """
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

    private const string ExpandMap = """
                                     #######
                                     #...#.#
                                     #.....#
                                     #..00@#
                                     #..0..#
                                     #.....#
                                     #######
                                     """;

    private const string ExpandMap1 = """
                                     ##############
                                     ##......##..##
                                     ##..........##
                                     ##...[][]@..##
                                     ##....[]....##
                                     ##..........##
                                     ##############
                                     """;

    private const string ExpandMap2 = """
                                      ##############
                                      ##......##..##
                                      ##..........##
                                      ##...[][]...##
                                      ##....[].@..##
                                      ##..........##
                                      ##############
                                      """;

    private const string ExpandMap3 = """
                                      ##############
                                      ##......##..##
                                      ##..........##
                                      ##...[][]...##
                                      ##....[]....##
                                      ##.......@..##
                                      ##############
                                      """;

    private const string ExpandMap4 = """
                                      ##############
                                      ##......##..##
                                      ##..........##
                                      ##...[][]...##
                                      ##....[]....##
                                      ##......@...##
                                      ##############
                                      """;

    private const string ExpandMap5 = """
                                      ##############
                                      ##......##..##
                                      ##..........##
                                      ##...[][]...##
                                      ##....[]....##
                                      ##.....@....##
                                      ##############
                                      """;

    private const string ExpandMap6 = """
                                      ##############
                                      ##......##..##
                                      ##...[][]...##
                                      ##....[]....##
                                      ##.....@....##
                                      ##..........##
                                      ##############
                                      """;

    private const string ExpandMap7 = """
                                      ##############
                                      ##......##..##
                                      ##...[][]...##
                                      ##....[]....##
                                      ##.....@....##
                                      ##..........##
                                      ##############
                                      """;

    private const string ExpandMap8 = """
                                      ##############
                                      ##......##..##
                                      ##...[][]...##
                                      ##....[]....##
                                      ##....@.....##
                                      ##..........##
                                      ##############
                                      """;

    private const string ExpandMap9 = """
                                      ##############
                                      ##......##..##
                                      ##...[][]...##
                                      ##....[]....##
                                      ##...@......##
                                      ##..........##
                                      ##############
                                      """;

    private const string ExpandMap10 = """
                                      ##############
                                      ##......##..##
                                      ##...[][]...##
                                      ##...@[]....##
                                      ##..........##
                                      ##..........##
                                      ##############
                                      """;

    private const string ExpandMap11 = """
                                       ##############
                                       ##...[].##..##
                                       ##...@.[]...##
                                       ##....[]....##
                                       ##..........##
                                       ##..........##
                                       ##############
                                       """;

    private const string ExpandSmallMap1 = """
                                          .0@
                                          """;

    private const string ExpandSmallMap1_1 ="""
                                            .[]@..
                                            """;
    
    private const string ExpandSmallMap2 = """
                                           ...
                                           .0@
                                           00.
                                           ...
                                           """;

    private const string ExpandSmallMap2_1 ="""
                                            .[]...
                                            ..[]..
                                            []@...
                                            ......
                                            """;
    [Theory]
    [InlineData(SmallMap, "<", SmallMap1, 1, null)]
    [InlineData(SmallMap1, "^", SmallMap2, 1, null)]
    [InlineData(SmallMap2, "^", SmallMap3, 1, null)]
    [InlineData(SmallMap3, ">", SmallMap4, 1, null)]
    [InlineData(SmallMap4, ">", SmallMap5, 1, null)]
    [InlineData(SmallMap5, ">", SmallMap6, 1, null)]
    [InlineData(SmallMap6, "v", SmallMap7, 1, null)]
    [InlineData(SmallMap7, "v", SmallMap8, 1, null)]
    [InlineData(SmallMap8, "<", SmallMap9, 1, null)]
    [InlineData(SmallMap9, "v", SmallMap10, 1, null)]
    [InlineData(SmallMap10, ">", SmallMap11, 1, null)]
    [InlineData(SmallMap11, ">", SmallMap12, 1, null)]
    [InlineData(SmallMap12, "v", SmallMap13, 1, null)]
    [InlineData(SmallMap13, "<", SmallMap14, 1, null)]
    [InlineData(SmallMap14, "<", SmallMap15, 1, 2028)]
    [InlineData(MedMap, MedMoves, ExpectedMedMap, 1, 10092)]
    [InlineData(ExpandMap, "<", ExpandMap1, 2, null)]
    [InlineData(ExpandMap, "<v", ExpandMap2, 2, null)]
    [InlineData(ExpandMap, "<vv", ExpandMap3, 2, null)]
    [InlineData(ExpandMap, "<vv<", ExpandMap4, 2, null)]
    [InlineData(ExpandMap, "<vv<<", ExpandMap5, 2, null)]
    [InlineData(ExpandMap, "<vv<<^", ExpandMap6, 2, null)]
    [InlineData(ExpandMap, "<vv<<^^", ExpandMap7, 2, null)]
    [InlineData(ExpandMap, "<vv<<^^<", ExpandMap8, 2, null)]
    [InlineData(ExpandMap, "<vv<<^^<<", ExpandMap9, 2, null)]
    [InlineData(ExpandMap, "<vv<<^^<<^", ExpandMap10, 2, null)]
    [InlineData(ExpandMap, "<vv<<^^<<^^", ExpandMap11, 2, null)]
    [InlineData(ExpandSmallMap1, "<", ExpandSmallMap1_1, 2, null)]
    [InlineData(ExpandSmallMap2, "<>vv<<^", ExpandSmallMap2_1, 2, null)]
    public void TestRobotMoves(string initMap, string moves, string expectedMap, int scale, int? expectedSum)
    {
        var map = Solution.LinesToMap(initMap.Split('\n', StringSplitOptions.RemoveEmptyEntries), scale); 
        _testOutputHelper.WriteLine(map.Render());
        foreach (var m in map.Apply(moves))
        {
            var mapString = map.Render();
            _testOutputHelper.WriteLine($"{m}");
            _testOutputHelper.WriteLine(mapString);
        }
        _testOutputHelper.WriteLine(expectedMap);
        Assert.Equal(expectedMap.ReplaceLineEndings(""), map.Render().ReplaceLineEndings(""));
        if (!expectedSum.HasValue) return;
        var sum = Solution.Calculate(map);
        Assert.Equal(expectedSum, sum);
    }
}