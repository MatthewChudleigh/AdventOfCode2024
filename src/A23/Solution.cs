using System.Collections;
using System.Numerics;

namespace A23;

public static class Solution
{
    public static string SolvePart2(Dictionary<string, HashSet<string>> dict)
    {
        var idx = 0;
        var index = new Dictionary<string, int>();
        var rev = new Dictionary<int, string>();
        var nodes = new List<BitArray>();

        foreach (var a in dict)
        {
            if (!index.TryGetValue(a.Key, out var i))
            {
                i = idx;
                index[a.Key] = i;
                rev[i] = a.Key;
                idx++;
            }
            
            Console.WriteLine($"{a.Key}: {String.Join(", ", a.Value)}");

            var d = new BitArray(dict.Keys.Count);
            d.Set(i, true);

            foreach (var b in a.Value)
            {
                if (!index.TryGetValue(b, out var j))
                {
                    j = idx;
                    index[b] = j;
                    rev[j] = b;
                    idx++;
                }
                
                d.Set(j, true);
            }
            
            Console.WriteLine(String.Join(", ", d.ToIndexes().Select(x => rev[x])));
            Console.WriteLine();
            
            nodes.Add(d);
        }

        var largest = new List<int>();
        for (var i = 0; i < nodes.Count; i++)
        {
            var node = Iterate(rev, nodes, nodes[i], i);
            
            if (node.Count > largest.Count)
            {
                largest = node;
            }
        }

        return string.Join(",", largest.Select(i => rev[i]).Order());
    }

    public static List<int> Iterate(Dictionary<int, string> rev, List<BitArray> nodes, BitArray node, int index)
    {
        var largest = new List<int>();

        var fin = true;
        for (var i = index+1; i < nodes.Count; i++)
        {
            if (!node[i]) continue;
            fin = false;
            var n = (node.Clone() as BitArray)!.And(nodes[i]);
            
            Console.Write($"{index}: {i} ({rev[i]}): ");
            foreach (var z in node.ToIndexes()) { Console.Write(rev[z]); }
            Console.Write(": ");
            foreach (var z in n.ToIndexes()) { Console.Write(rev[z]); }
            Console.WriteLine();
            
            var l = Iterate(rev, nodes, n, i);
            if (largest.Count < l.Count)
            {
                largest = l;
            }
        }
    
        if (fin)
        {
            return node.ToIndexes();
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