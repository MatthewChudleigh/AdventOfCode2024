// See https://aka.ms/new-console-template for more information

using A19;

var baseDir = Environment.GetEnvironmentVariable("AOC_BaseDir");
var data = File.ReadAllLines(Path.Combine(baseDir!, "A19.data.txt"));
var towels = Solution.Load(data);
var count = Solution.PossibleDesigns(towels);
Console.WriteLine(count);