namespace A19;

public static class Solution
{
    // white (w), blue (u), black (b), red (r), or green (g)

    public class Onsen(string towels, ICollection<string> designs)
    {
        public List<string> Towels { get; } = towels.Split(",", StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.Trim())
            .OrderByDescending(x => x.Length).ThenBy(x => x).ToList();
        public ICollection<string> Designs { get; set; } = designs;
        
        public long IsPossible(string design, int index)
        {
            var checkedIndex = new Dictionary<int, long>();

            return Loop(index);
            
            long Loop(int idx)
            {
                if (checkedIndex.TryGetValue(idx, out var n)) return n;
                if (idx >= design.Length)
                {
                    return 1L;
                }

                var m = 0L;
                foreach (var towel in Towels)
                {
                    if (design.AsSpan()[idx..].StartsWith(towel))
                    {
                        var x = idx + towel.Length;
                        var c = Loop(x);
                        checkedIndex[x] = c;
                        m += c;
                    }
                }

                return m;
            }
        }
    }

    public static Onsen Load(string[] data)
    {
        var towels = new Onsen(data[0], data.Skip(1).Where(d => !string.IsNullOrWhiteSpace(d)).ToList());
        return towels;
    }

    public static (int Possible, long Permute) PossibleDesigns(Onsen onsen)
    {
        var n = onsen.Designs.Select(t => onsen.IsPossible(t, 0)).ToList();
        return (n.Count(x => x > 0), n.Sum());
    }
}