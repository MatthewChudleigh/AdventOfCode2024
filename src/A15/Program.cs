using A15;

var baseDir = Environment.GetEnvironmentVariable("AOC_BaseDir");
var data = File.ReadAllLines(Path.Combine(baseDir!, "A15.data.txt"));
var moves = String.Join("",File.ReadAllLines(Path.Combine(baseDir!, "A15.moves.txt")));
var map = Solution.LinesToMap(data);
map.Apply(moves);
Console.WriteLine(Solution.Calculate(map));