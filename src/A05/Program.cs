/*
X|Y :
- if both page number X and page number Y are to be produced as part of an update
- page number X must be printed [at some point] *before* page number Y.

Input: page ordering rules and the pages to produce in each update
[Px|Py,...],[Pn,...]

- Determine which update has the pages in the right order.
- Sum the middle page number from those correctly-ordered updates?
*/
var dataPath = "/workspaces/AdventOfCode2024/data/A05/A05.1.txt";

var sumTotal = A05.Analyse(dataPath);
Console.WriteLine(sumTotal);

static class A05
{

    public static int Analyse(string dataPath)
    {
        int sumTotal = 0;
        var orderings = new Dictionary<int, HashSet<int>>();

        var updates = new List<List<int>>();
        foreach (var line in File.ReadAllLines(dataPath))
        {
            if (line.Contains("|"))
            {
                var xy = line.Split("|", StringSplitOptions.RemoveEmptyEntries);
                var x = Int32.Parse(xy[0]);
                var y = Int32.Parse(xy[1]);
                if (!orderings.TryGetValue(x, out var lt))
                {
                    lt = new HashSet<int>();
                    orderings[x] = lt;
                }
                lt.Add(y);
            }
            else if (line.Contains(","))
            {
                var pages = line.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(Int32.Parse).ToList();
                updates.Add(pages);
            }
        }

        foreach (var pages in updates)
        {
            bool isOrderedOk = true;
            for (var i = 0; i < pages.Count - 1; ++i)
            {
                if ((orderings.TryGetValue(pages[i], out var ordering) && !ordering.Contains(pages[i + 1])) ||
                    (orderings.TryGetValue(pages[i + 1], out var ordering2) && ordering2.Contains(pages[i])))
                {
                    isOrderedOk = false;
                    break;
                }
            }

            if (!isOrderedOk)
            {
                pages.Sort((lhs, rhs) =>
                {
                    return (orderings.ContainsKey(lhs) && orderings[lhs].Contains(rhs)) ? -1 :
                        (orderings.ContainsKey(rhs) && orderings[rhs].Contains(lhs)) ? 1 :
                        -1;
                });

                sumTotal += pages[pages.Count / 2];
            }
        }

        return sumTotal;
    }
}