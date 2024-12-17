using A17;

var baseDir = Environment.GetEnvironmentVariable("AOC_BaseDir");
var data = File.ReadAllLines(Path.Combine(baseDir!, "A17.data.txt"));
var targetOutput = new List<ushort>() { 2, 0, 4, 2, 7, 0, 1, 0, 3 };
//var targetOutput = new List<ushort>() { 0, 3, 5, 4, 3, 0 };
var computer = Solution.Load(data, null);
var q = Solution.Quine(computer.Instructions);
Console.WriteLine(q);

computer.Calculate();
Console.WriteLine(string.Join(",", computer.Output));
