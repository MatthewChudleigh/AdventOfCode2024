// See https://aka.ms/new-console-template for more information

using A20;

var baseDir = Environment.GetEnvironmentVariable("AOC_BaseDir");
var data = File.ReadAllLines(Path.Combine(baseDir!, "A20.data.txt"));
var track = Solution.Load(data);
track.Calc();
var count = track.Points[track.End];
Console.WriteLine(count);
var cheats = track.Cheats(20);

var savings = 0;
foreach (var c in cheats.OrderByDescending(kv => kv.Key).ThenBy(kv => kv.Value))
{
    Console.WriteLine($"{c.Value} : {c.Key}");
    if (c.Key >= 100) savings += c.Value;
}

Console.WriteLine(savings);