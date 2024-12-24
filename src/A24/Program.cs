/*
 * The device seems to be trying to produce a number through some boolean logic gates.
 * Each gate has two inputs and one output.
 * The gates all operate on values that are either true (1) or false (0).

   AND gates output 1 if both inputs are 1; if either input is 0, these gates output 0.
   OR gates output 1 if one or both inputs is 1; if both inputs are 0, these gates output 0.
   XOR gates output 1 if the inputs are different; if the inputs are the same, these gates output 0.

Gates wait until both inputs are received before producing output;
wires can carry 0, 1 or no value at all.
There are no loops; once a gate has determined its output, the output will not change until the whole system is reset.

Each wire is connected to at most one gate output, but can be connected to many gate inputs.

x00: 1
x01: 1
x02: 1
y00: 0
y01: 1
y02: 0

x00 AND y00 -> z00
x01 XOR y01 -> z01
x02 OR y02 -> z02
 */

using System.Collections;
using System.Numerics;

var baseDir = Environment.GetEnvironmentVariable("AOC_BaseDir");
var data = File.ReadAllLines(Path.Combine(baseDir!, "A24.data.txt"));

var values = data.Where(d => d.Contains(':'))
    .Select(d => d.Split(':'))
    .ToDictionary(d => d[0], d => Int32.Parse(d[1]));

var queue = new Queue<(string reg, string lhs, string op, string rhs)>();
foreach (var d in data.Where(d => d.Contains("->")))
{
    var a = d.Split("->");
    var b = a[0].Split(' ', StringSplitOptions.RemoveEmptyEntries);
    var l = b[0].Trim();
    var r = b[2].Trim();
    var reg = a[1].Trim();
    var op = b[1].Trim();
    
    queue.Enqueue((reg, l, op, r));
}

while (queue.TryDequeue(out var d))
{
    if (values.TryGetValue(d.lhs, out var lhs) && values.TryGetValue(d.rhs, out var rhs))
    {
        values[d.reg] = d.op switch
        {
            "OR" => lhs | rhs,
            "XOR" => lhs ^ rhs,
            "AND" => lhs & rhs,
            _ => throw new ArgumentException($"Unknown operator: {d.op}")
        };
    }
    else
    {
        queue.Enqueue(d);
    }
}

var x = Solution.ToVal('x', values);
var y = Solution.ToVal('y', values);
var z = Solution.ToVal('z', values);
Console.WriteLine(x);
Console.WriteLine(y);
Console.WriteLine(z);

Console.WriteLine(Solution.Bin(x));
Console.WriteLine(Solution.Bin(y));
Console.WriteLine("...");
Console.WriteLine(Solution.Bin(x+y));
Console.WriteLine(Solution.Bin(z));

public static class Solution
{
    public static BigInteger ToVal(char z, Dictionary<string, int> v)
    {
        BigInteger result = 0L;
        foreach (var i in v.Where(kv => kv.Key.StartsWith(z) && kv.Value == 1).Select(kv => int.Parse(kv.Key[1..])))
        {
            result += BigInteger.Pow(2, i);
        }

        return result;
    }

    public static string Bin(BigInteger n)
    {
        var s = new Stack<char>();
        while (n != 0)
        {
            s.Push((n & 0b1) == 1 ? '1' : '0');
            n >>= 1;
        }

        return String.Join("", s);
    }
}

// z12/z13
