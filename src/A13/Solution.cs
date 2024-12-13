using System.Text.RegularExpressions;

namespace A13;

public static class Solution
{
    public class Machine
    {
        public (long X, long Y) A { get; set; }
        public (long X, long Y) B { get; set; }
        public (long X, long Y) Prize { get; set; }
    }
    
    public static long Solve(IEnumerable<string> data, long offset)
    {
        var machines = GetMachines(data, offset);
        
        return Solve(machines);
    }

    public static long Solve(IEnumerable<Machine> machines)
    {
        long solution = 0;
        foreach (var machine in machines)
        {
            var total = Solve(machine);
            Console.WriteLine(total);
            solution += total;
        }
        Console.WriteLine("...");
        return solution;
    }

    public static long Solve(Machine machine)
    {
/*
Cx = ax * AN + bx * BN
Cy = ay * AN + by * BN
*/ 
        var a = machine.A;
        var b = machine.B;
        var p = machine.Prize;

        var num = (p.X * b.Y - p.Y * b.X);
        var den = (a.X * b.Y - b.X * a.Y);

        if (num % den != 0) return 0;
        
        var A = num / den;
        var B = (p.Y - a.Y*A) / b.Y;
        
        return A*3+B;
    }
    
    public static List<Machine> GetMachines(IEnumerable<string> data, long offset)
    {
        var machines = new List<Machine>();
        foreach (var lines in data.Chunk(3))
        {
            var bA = Re.ReButton().Match(lines[0]);
            var bB = Re.ReButton().Match(lines[1]);
            var pz = Re.RePrize().Match(lines[2]);
            var machine = new Machine()
            {
                A = (long.Parse(bA.Groups["x"].Value), long.Parse(bA.Groups["y"].Value)), 
                B = (long.Parse(bB.Groups["x"].Value), long.Parse(bB.Groups["y"].Value)),
                Prize = (long.Parse(pz.Groups["x"].Value)+offset, long.Parse(pz.Groups["y"].Value)+offset),
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

