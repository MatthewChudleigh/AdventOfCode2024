// See https://aka.ms/new-console-template for more information

using A13;

var baseDir = Environment.GetEnvironmentVariable("AOC_BaseDir");
var data = File.ReadAllLines(Path.Combine(baseDir!, "A13.data.txt")).Where(x => !string.IsNullOrWhiteSpace(x));
var cost = Solution.Solve(data, 10000000000000);
Console.WriteLine(cost);