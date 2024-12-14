// See https://aka.ms/new-console-template for more information

using A14;

var baseDir = Environment.GetEnvironmentVariable("AOC_BaseDir");
var data = File.ReadAllLines(Path.Combine(baseDir!, "A14.data.txt")).Where(x => !string.IsNullOrWhiteSpace(x));
var cost = Solution.Solve(data, 100000, 101, 103);
Console.WriteLine(cost);