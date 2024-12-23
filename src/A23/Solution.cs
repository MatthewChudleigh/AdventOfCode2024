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
            
            nodes.Add(d);
        }

        var largest = new List<int>();
        for (var i = 0; i < nodes.Count; i++)
        {
            var node = Iterate(nodes, nodes[i], i+1);
            
            if (node.Count > largest.Count)
            {
                largest = node;
            }
        }

        return string.Join(",", largest.Select(i => rev[i]).Order());
    }

    public static List<int> Iterate(List<BitArray> nodes, BitArray node, int index){
        var largest = new List<int>();
        if (index >= node.Count)
        {
            return node.ToIndexes();
        }
        
        for (var i = index; i < nodes.Count; i++)
        {
            if (!node[i]) continue;
            var n = nodes[i].And(node);
            if (!node[i - 1]) continue;
            
            var l = Iterate(nodes, n, i + 1);
            if (l.Count > largest.Count)
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