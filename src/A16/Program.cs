using A16;

var baseDir = Environment.GetEnvironmentVariable("AOC_BaseDir");
var data = File.ReadAllLines(Path.Combine(baseDir!, "A16.data.txt"));
var map = Solution.LinesToMap(data);
Console.WriteLine(Solution.CalculateMinScore(map));