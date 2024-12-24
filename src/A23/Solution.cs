namespace A23;

public static class Solution
{
    public static bool IsFullyConnected(Dictionary<string, List<string>> graph, List<string> nd)
    {
        for (var i = 0; i < nd.Count - 1; i++)
        {
            for (var j = i + 1; j < nd.Count; j++)
            {
                var n1 = nd[i];
                var n2 = nd[j];
                if (!graph[n1].Contains(n2)) return false;
            }
        }

        return true;
    }

    public static void Solve(string[] lines)
    {
        List<string> tComp = [];
        Dictionary<string, List<string>> graph = new();
        foreach (var line in lines.Select(l => l.Split("-")))
        {
            foreach (var l in line)
            {
                if (graph.ContainsKey(l)) continue;
                graph[l] = [];
                if (l[0] == 't') tComp.Add(l);
            }

            graph[line[0]].Add(line[1]);
            graph[line[1]].Add(line[0]);
        }

        var count = tComp.SelectMany(
                tn =>
                    graph[tn].SelectMany(n1 =>
                        from n2 in graph[tn]
                        where n1 != n2
                        where graph[n1].Contains(n2)
                        select (List<string>) [tn, n1, n2]))
            .Select(g => string.Join("", g.Order()))
            .Distinct()
            .Count();

        var maxPwd = "";
        var nodes = graph.Keys.OrderBy(k => graph[k].Count).ToList();
        foreach (var n1 in nodes)
        {
            foreach (var nb in graph[n1])
            {
                List<string> group =
                [
                    n1,
                    nb
                ];
                group.AddRange(graph[n1].Where(nb2 => graph[nb].Contains(nb2)));

                if (!IsFullyConnected(graph, group)) continue;
                var pwd = string.Join(",", group.Order());
                if (pwd.Length > maxPwd.Length)
                {
                    maxPwd = pwd;
                }
                break;
            }

        }

        Console.WriteLine($"{count}");
        Console.WriteLine($"{maxPwd}");
    }
}