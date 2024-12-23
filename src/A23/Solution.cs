namespace A23;

public static class Solution
{
    public static int Solve(char t, string[] data)
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
}