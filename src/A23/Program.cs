// See https://aka.ms/new-console-template for more information

using A23;

var baseDir = Environment.GetEnvironmentVariable("AOC_BaseDir");

var data = File.ReadAllLines(Path.Combine(baseDir!, "A23.data.txt"));
Solution.Solve(data);