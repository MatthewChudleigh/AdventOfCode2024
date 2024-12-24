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

BigInteger result = 0L;

var i = 0;
var sb = new Stack<int>();
foreach (var z in values.
             Where(v => v.Key.StartsWith('z'))
             .Select(v => (int.Parse(v.Key[1..]), v.Value))
             .Where(z => z.Value > 0).OrderBy(z => z.Item1))
{
    while (i < z.Item1)
    {
        sb.Push(0);
        i++;
    }
    sb.Push(z.Value);
    i = z.Item1 + 1;
}
Console.WriteLine(string.Join("",sb));
// 64755511006320
Console.WriteLine(result);