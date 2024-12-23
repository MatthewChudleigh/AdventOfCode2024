// See https://aka.ms/new-console-template for more information

using A23;

var baseDir = Environment.GetEnvironmentVariable("AOC_BaseDir");

var data = File.ReadAllLines(Path.Combine(baseDir!, "A23.data.txt"));

var count = Solution.Solve('t', data);
Console.WriteLine(count);
var dict = Solution.ToDict(data);
var password = Solution.SolvePart2(dict);
Console.WriteLine(password);