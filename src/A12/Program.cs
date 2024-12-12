// See https://aka.ms/new-console-template for more information

using A12;
var baseDir = Environment.GetEnvironmentVariable("AOC_BaseDir");
var cost = Solution.Solve(@$"{baseDir}/data/A12.data.txt");
Console.WriteLine(cost);
