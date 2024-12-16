using A15;

var baseDir = Environment.GetEnvironmentVariable("AOC_BaseDir");
var data = File.ReadAllLines(Path.Combine(baseDir!, "A15.data.txt"));
var moves = String.Join("",File.ReadAllLines(Path.Combine(baseDir!, "A15.moves.txt")));
var map = Solution.LinesToMap(data, 2);
var file = new StreamWriter(Path.Combine(baseDir!, "A15.out.txt"));
file.WriteLine(map.Render());
foreach (var m in map.Apply(moves))
{
    var render = map.Render();
    file.WriteLine(m);
    file.WriteLine(render);
}
file.Flush();
Console.WriteLine(Solution.Calculate(map));