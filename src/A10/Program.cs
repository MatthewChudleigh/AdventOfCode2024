/*
Topographic map
0123
1234
8765
9876

0 (lowest) - 9 (highest)

- Trails:
  - As long as possible: all end at 9
  - Even, gradual, uphill slope: +1 per step
  - Only move U,D,L,R
- Trailhead:
  - Starts 1+ trails
  - Always start at 0
  - Score: count of 9-height positions reachable
  
- Sum total trailhead scores
 */

using A10;

var score = Solution.Solve(@"data\A10.data.txt", true);
Console.WriteLine(score);