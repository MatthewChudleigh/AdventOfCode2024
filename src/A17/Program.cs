using A17;

var baseDir = Environment.GetEnvironmentVariable("AOC_BaseDir");
var data = File.ReadAllLines(Path.Combine(baseDir!, "A17.data.txt"));
var computer = Solution.Load(data);
computer.Calculate();
Console.WriteLine(String.Join(",", computer.Output));