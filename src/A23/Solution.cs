namespace A23;

public static class Solution
{
    public static int Solve(char t, string[] data)
    {
        var dict = ToDict(data);
        return Solve(t, dict);
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

    public static string SolvePart2(Dictionary<string, HashSet<string>> dict)
    {
        List<string> largest = [];
        var check = new HashSet<string>();
        foreach (var kv in dict)
        {
            check.Add(kv.Key);
            foreach (var v in kv.Value.Where(v => !check.Contains(v)))
            {
                var code = Calculate(dict, [v, kv.Key]);
                if (code.Count > largest.Count)
                {
                    largest = code;
                }
            }
        }

        return string.Join(",", largest.Order());
    }

    static List<string> Calculate(Dictionary<string, HashSet<string>> dict, List<string> set)
    {
        var largest = set;
        foreach (var v in dict[set[0]])
        {
            if(set.Contains(v)) continue;
            if(set.Any(s => !dict[v].Contains(s))) continue;
            var l = Calculate(dict, [v, ..set]);
            if (l.Count > largest.Count)
            {
                largest = l;
            }
        }
        
        return largest;
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
}