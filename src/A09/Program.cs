/*
The disk map uses a dense format to represent the layout of files and free space on the disk.
The digits *alternate* between:
- length of a file
- length of free space

- 12345:
 - one-block file
 - two blocks of free space
 - a three-block file
 - four blocks of free space
 - five-block file
 
 
Each file on disk also has an ID number based on the order of the files as they appear before they are rearranged, starting with ID 0
- disk map 12345 has three files: 
  - a one-block file with ID 0, a three-block file with ID 1, and a five-block file with ID 2
  - Using one character for each block where digits are the file ID and . is free space, the disk map 12345 represents these individual blocks:
  12345
  0..111....22222
  
  2333133121414131402
  00...111...2...333.44.5555.6666.777.888899
  
- move file blocks *one at a time from the end of the disk to the leftmost free space block* until there are no gaps remaining between file blocks
- For the disk map 12345, the process looks like this: 
0..111....22222
02.111....2222.
022111....222..
0221112...22...
02211122..2....
022111222......

- calculate the checksum:
  - add up the result of *multiplying* each of these blocks' position with the file ID number it contains
  - The leftmost block is in position 0
  - If a block contains free space, skip it instead
    00998111
    ********
    01234567
 */

using A09;

//var checksum = Solution.Checksum(@"D:\Source\AdventOfCode2024data/A09\A09.test-0.txt");
//Console.WriteLine(checksum);
var checksum = Solution.Checksum(@"data/A09.test-0.txt", false);
Console.WriteLine(checksum);
checksum = Solution.Checksum(@"data/A09.data.txt", false);
Console.WriteLine(checksum);

checksum = Solution.Checksum(@"data/A09.test-0.txt", true);
Console.WriteLine(checksum);
checksum = Solution.Checksum(@"data/A09.data.txt", true);
Console.WriteLine(checksum);