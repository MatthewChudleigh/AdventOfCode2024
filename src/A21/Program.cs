// See https://aka.ms/new-console-template for more information


using A21;

var baseDir = Environment.GetEnvironmentVariable("AOC_BaseDir");
var codes = File.ReadAllLines(Path.Combine(baseDir!, "A21.data.txt"));
var robots = 26;
var cost = codes.Select(code => Solution.Calculate(robots, code, false)).Sum(c => c.Cost);

Console.WriteLine(cost);