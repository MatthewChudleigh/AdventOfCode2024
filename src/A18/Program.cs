
using A18;

var baseDir = Environment.GetEnvironmentVariable("AOC_BaseDir");
var data = File.ReadAllLines(Path.Combine(baseDir!, "A18.data.txt"));

var minSteps = Solution.Solve(71, 71, data, 1024);
Console.WriteLine(minSteps);