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
var graph = Path.Combine(baseDir!, "A24.dot");

var values = data.Where(d => d.Contains(':'))
    .Select(d => d.Split(':'))
    .ToDictionary(d => d[0], d => Int32.Parse(d[1]));

var computer = new Dictionary<string, (string lhs, string op, string rhs)>();
var queue = new Queue<(string reg, string lhs, string op, string rhs)>();
foreach (var d in data.Where(d => d.Contains("->")))
{
    var a = d.Split("->");
    var b = a[0].Split(' ', StringSplitOptions.RemoveEmptyEntries);
    var l = b[0].Trim();
    var r = b[2].Trim();

    if (String.Compare(l, r, StringComparison.Ordinal) > 0)
    {
        (r, l) = (l, r);
    }
    
    var reg = a[1].Trim();
    var op = b[1].Trim() switch
    {
        "OR" => "|",
        "XOR" => "^",
        "AND" => "&",
        _ => throw new ArgumentOutOfRangeException()
    };
    
    queue.Enqueue((reg, l, op, r));
    computer[reg] = (l, op, r);
}

foreach (var v in values)
{
    computer[v.Key] = ("0", v.Value == 0 ? "&" : "|", "1");
}

while (queue.TryDequeue(out var d))
{
    if (values.TryGetValue(d.lhs, out var lhs) && values.TryGetValue(d.rhs, out var rhs))
    {
        values[d.reg] = d.op switch
        {
            "|" => lhs | rhs,
            "^" => lhs ^ rhs,
            "&" => lhs & rhs,
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

//Solution.Graph(graph, computer);
//Solution.Print(computer);
Solution.Analyse(computer);

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

    public static void Analyse(Dictionary<string, (string lhs, string op, string rhs)> computer)
    {
        var node = "kvh";
        var znode = "z38";
        var op = computer[node];
        var zop = computer[znode];
        var lhs = computer[op.lhs];
        var rhs = computer[op.rhs];

        if (rhs.lhs.StartsWith("x") || rhs.lhs.StartsWith("y"))
        {
            (lhs, rhs) = (rhs, lhs);
        }

        var rll = rhs.lhs;
        var rrr = rhs.rhs;
        var rl = computer[rhs.lhs];
        var rr = computer[rhs.rhs];
        
        if (rl.op != "^")
        {
            (rr, rl) = (rl, rr);
            rrr = rhs.lhs;
            rll = rhs.rhs;
        }
        
        if (op.op != "|") { Console.WriteLine($"0: {node}: | : {op}"); }
        if (zop.op != "^") { Console.WriteLine($"1: {node}: ^: {zop}"); }

        if (lhs.op != "&") { Console.WriteLine($"2: {node}: & : {lhs}"); }
        if (!lhs.lhs.StartsWith("x") || !lhs.rhs.StartsWith("y")) { Console.WriteLine($"4: {node}: xy : {lhs}"); }
        
        if (rhs.op != "&") { Console.WriteLine($"3: {node}: & : {rhs}"); }
        if (rll != zop.lhs || rrr != zop.rhs) { Console.WriteLine($"5: {node}: zop {zop} : {rhs}"); }

        if (rl.op != "^") { Console.WriteLine($"6: {rll}: ^: {rl}"); }
        if (rl.lhs != lhs.lhs || rl.rhs != lhs.rhs) { Console.WriteLine($"7: {rll}: lhs {lhs} : {rl}"); }
        
        if (rr.op != "|") { Console.WriteLine($"8: {rrr}: |: {rr}"); }

        node = rrr;
        Console.WriteLine(node);
    }

    public static void Print(Dictionary<string, (string lhs, string op, string rhs)> computer)
    {
        
        foreach (var zOp in computer.Where(kv => kv.Key.StartsWith('z')).Select(kv => kv.Key).Order())
        {
            var ops = new List<string>();
            var inputs = new List<string>();
            var stack = new Queue<string>();
            stack.Enqueue(zOp);
    
            while (stack.TryDequeue(out var op))
            {
                if (op.StartsWith('x') || op.StartsWith('y'))
                {
                    inputs.Add(op);
                }
                else
                {
                    var rOp = computer[op];
                    ops.Add($"{rOp.lhs} {rOp.op} {rOp.rhs}");
                    stack.Enqueue(rOp.lhs);
                    stack.Enqueue(rOp.rhs);
                }
            }
    
            var xI = inputs.Where(i => i.StartsWith('x')).Select(i => i[1..]).Order();
            var yI = inputs.Where(i => i.StartsWith('y')).Select(i => i[1..]).Order();
            //Console.WriteLine($"{zOp}:\nX: {string.Join(" ", xI)}\nY: {string.Join(" ", yI)}");

            ops.Reverse();
            //Console.WriteLine($"{zOp}: {string.Join(" ", ops)}");
        }

    }

    public static void Graph(string file, Dictionary<string, (string lhs, string op, string rhs)> computer)
    {
        using var sw = new StreamWriter(file);
        sw.WriteLine("digraph operations {");
        foreach (var c in computer)
        {
            var op = c.Value.op switch { "|" => "or", "&" => "and", "^" => "xor", _ => "nop"};
            sw.WriteLine($"    {c.Value.lhs} -> {c.Value.lhs}_{op}_{c.Value.rhs};");
            sw.WriteLine($"    {c.Value.rhs} -> {c.Value.lhs}_{op}_{c.Value.rhs};");
            sw.WriteLine($"    {c.Value.lhs}_{op}_{c.Value.rhs} -> {c.Key};");
        }

        sw.WriteLine("}");
    }
}

// z12/z13
