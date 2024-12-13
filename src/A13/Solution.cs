using System.Text.RegularExpressions;

namespace A13;

public static class Solution
{
    public class Machine
    {
        public (int X, int Y) A { get; set; }
        public (int X, int Y) B { get; set; }
        public (int X, int Y) Prize { get; set; }
    }
    
    public static int Solve(IEnumerable<string> data)
    {
        var machines = GetMachines(data);
        
        return Solve(machines);
    }

    public static int Solve(IEnumerable<Machine> machines)
    {
        var solution = 0;
        foreach (var machine in machines)
        {
            solution += Solve(machine);
        }
        
        return solution;
    }

    public static int Solve(Machine machine)
    {
/*
Cx = ax * AN + bx * BN
Cy = ay * AN + by * BN
*/
        var combinations  = new Dictionary<(int a, int b), int>();
        for (var a = 0; a < 100; ++a)
        {
            for (var b = 0; b < 100; ++b)
            {
                combinations[(a, b)] = a * 3 + b;
            }
        }

        foreach (var kv in combinations.OrderBy(c => c.Value))
        {
            var (a, b) = kv.Key;
            if (machine.A.X*a+machine.B.X*b == machine.Prize.X && 
                 machine.A.Y*a+machine.B.Y*b == machine.Prize.Y)
            {
                return kv.Value;
            }
        }

        return 0;
    }
    
    public static List<Machine> GetMachines(IEnumerable<string> data)
    {
        var machines = new List<Machine>();
        foreach (var lines in data.Chunk(3))
        {
            var bA = Re.ReButton().Match(lines[0]);
            var bB = Re.ReButton().Match(lines[1]);
            var pz = Re.RePrize().Match(lines[2]);
            var machine = new Machine()
            {
                A = (int.Parse(bA.Groups["x"].Value), int.Parse(bA.Groups["y"].Value)), 
                B = (int.Parse(bB.Groups["x"].Value), int.Parse(bB.Groups["y"].Value)),
                Prize = (int.Parse(pz.Groups["x"].Value), int.Parse(pz.Groups["y"].Value)),
            };
            machines.Add(machine);
        }
        return machines;
    }
}

public partial class Re
{
    [GeneratedRegex(@"Button [AB]: X\+(?<x>\d+), Y\+(?<y>\d+)", RegexOptions.Compiled)]
    public static partial Regex ReButton();
    
    [GeneratedRegex(@"Prize: X=(?<x>\d+), Y=(?<y>\d+)", RegexOptions.Compiled)]
    public static partial Regex RePrize();
}

