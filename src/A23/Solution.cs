using System.Collections;
using System.Numerics;

namespace A23;

public static class Solution
{
    public static string SolvePart2(Dictionary<string, HashSet<string>> dict)
    {
        var index = new Dictionary<string, int>();
        var rev = new Dictionary<int, string>();
        var nodes = new List<BitArray>();
        foreach (var ki in dict.Keys.Order().Select((k,i) => (k,i)))
        {
            nodes.Add(new BitArray(dict.Keys.Count));
            index[ki.k] = ki.i;
            rev[ki.i] = ki.k;
        }

        var z = 0;
        foreach (var a in dict.OrderBy(kv => kv.Key))
        {
            var i = index[a.Key];
            //Console.WriteLine($"{a.Key}: {String.Join(", ", a.Value)}");

            var d = nodes[z];
            d.Set(i, true);

            foreach (var b in a.Value)
            {
                var j = index[b];
                d.Set(j, true);
            }
            
            //Console.WriteLine(String.Join(", ", d.ToIndexes().Select(x => rev[x])));
            //Console.WriteLine();
            z++;
        }

        var largest = new List<int>();
        for (var i = 0; i < nodes.Count; i++)
        {
            var n = (nodes[i].Clone() as BitArray)!;
            for (var j = 0; j < i; ++j)
            {
                n.Set(j, false);
            }
            var node = Iterate(rev, nodes, n, i);
            //Console.WriteLine($"** {i} : {rev[i]} : {string.Join(",", node.Select(m => rev[m]))}");
            
            if (node.Count > largest.Count)
            {
                largest = node;
            }
        }

        return string.Join(",", largest.Select(i => rev[i]).Order());
    }

    public static List<int> Iterate(Dictionary<int, string> rev, List<BitArray> nodes, BitArray node, int index)
    {
        var fin = true;
        while (index + 1 < nodes.Count)
        {
            index++;
            if (!node[index]) continue;
            fin = false;
            break;
        }

        if (fin)
        {
            return node.ToIndexes();
        }
        
        var largest = new List<int>();

        for (var i = index; i < nodes.Count; i++)
        {
            if (!node[i]) continue;
            var n = (node.Clone() as BitArray)!.And(nodes[i]);
            /*
            Console.Write($"{index}: {i} ({rev[i]}): ");
            Console.Write(String.Join("", node.ToIndexes().Select(x => rev[x])));
            Console.Write(": ");
            Console.Write(String.Join("", nodes[i].ToIndexes().Select(x => rev[x])));
            Console.Write(": ");
            Console.Write(String.Join("", n.ToIndexes().Select(x => rev[x])));
            Console.WriteLine();
            */
            var l = Iterate(rev, nodes, n, i);
            if (largest.Count < l.Count)
            {
                largest = l;
            }
        }
    
        return largest;
    }
    
    public static List<int> ToIndexes(this BitArray array)
    {
        return array.Cast<bool>().Select((b, i) => (b, i)).Where(bi => bi.b).Select(bi => bi.i).ToList();
    }

    public static int Solve(char t, string[] data)
    {
        var dict = ToDict(data);
        return Solve(t, dict);
    }
    
    public static int Solve(char t, Dictionary<string, HashSet<string>> pairs)
    {
        var hashset = new HashSet<string>();
        foreach (var n0 in pairs.Where(k =>
                     k.Key.StartsWith(t)).SelectMany(k => k.Value.Select(v => (k.Key, v))))
        {
            foreach (var n1 in pairs[n0.v].Where(n => n != n0.Key).Where(n => pairs[n].Contains(n0.Key)))
            {
                List<string> s = [n0.Key, n0.v, n1];
                hashset.Add(string.Join("", s.Order()));
            }
        }

        return hashset.Count;
    }

    public static Dictionary<string, HashSet<string>> ToDict(string[] data)
    {
        var dict = new Dictionary<string, HashSet<string>>();
        foreach (var kv in data
                     .Select(l => l.Split('-')))
        {
            if(!dict.TryGetValue(kv[0], out var p0))
            {
                p0 = [];
                dict[kv[0]] = p0;
            }
            if(!dict.TryGetValue(kv[1], out var p1))
            {
                p1 = [];
                dict[kv[1]] = p1;
            }

            p0.Add(kv[1]);
            p1.Add(kv[0]);
        }

        return dict;
    }
}