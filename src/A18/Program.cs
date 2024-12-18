
using A18;

var baseDir = Environment.GetEnvironmentVariable("AOC_BaseDir");
var data = File.ReadAllLines(Path.Combine(baseDir!, "A18.data.txt"))
    .Select(line =>
    {
        var d = line.Split(",");
        return (Int32.Parse(d[0]), Int32.Parse(d[1]));
    }).ToArray();

var min = 2936;
var max = 2937;
var next = max - (max - min) / 2;
var minSteps = Solution.Solve(71, 71, data, next);
Console.WriteLine(minSteps);