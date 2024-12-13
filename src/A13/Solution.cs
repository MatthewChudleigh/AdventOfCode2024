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
        return GetMachines(data, offset).Sum(Solve);
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
    
    public static IEnumerable<Machine> GetMachines(IEnumerable<string> data, long offset)
    {
        return
            from lines in data.Chunk(3)
            let bA = Re.ReButton().Match(lines[0])
            let bB = Re.ReButton().Match(lines[1])
            let pz = Re.RePrize().Match(lines[2])
            select new Machine()
            {
                A = (long.Parse(bA.Groups["x"].Value), long.Parse(bA.Groups["y"].Value)), 
                B = (long.Parse(bB.Groups["x"].Value), long.Parse(bB.Groups["y"].Value)), 
                Prize = (long.Parse(pz.Groups["x"].Value) + offset, long.Parse(pz.Groups["y"].Value) + offset),
            };
    }
}

// ReSharper disable once ClassNeverInstantiated.Global
public partial class Re
{
    [GeneratedRegex(@"Button [AB]: X\+(?<x>\d+), Y\+(?<y>\d+)", RegexOptions.Compiled)]
    public static partial Regex ReButton();
    
    [GeneratedRegex(@"Prize: X=(?<x>\d+), Y=(?<y>\d+)", RegexOptions.Compiled)]
    public static partial Regex RePrize();
}

