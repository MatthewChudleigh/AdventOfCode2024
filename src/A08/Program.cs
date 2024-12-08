/*
- Antinode:
  - any point in line with two antennas of the same frequency
  - only when one of the antennas is *twice* as far away as the other
  - i.e., for any pair of antennas with the same frequency, there are two antinodes, one on either side of them.
  - antinodes can occur at locations that contain antennas

- How many unique locations within the bounds of the map contain an antinode?

  # AA #
  #  A  A  #
  #   A   A   #
  #    A    A    #
 */

var map = A08.Solution.ReadMap(@"D:\Source\AdventOfCode2024\data\A08\A08.1.txt");
Console.WriteLine($"{map.Width}x{map.Height}y");
var antiNodes = map.UniqueAntinodes(true);
Console.WriteLine(String.Join(Environment.NewLine, antiNodes));
Console.WriteLine(antiNodes.Count);
