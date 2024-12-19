namespace A19;

public static class Solution
{
    // white (w), blue (u), black (b), red (r), or green (g)

    public class Onsen(string towels, List<string> designs)
    {
        public List<string> Towels { get; } = towels.Split(",", StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.Trim())
            .OrderByDescending(x => x.Length).ThenBy(x => x).ToList();
        public ICollection<string> Designs { get; set; } = designs;
        public HashSet<string> Possible { get; set; } = new();
        public HashSet<string> NotPossible { get; set; } = new();
        public bool IsPossible(string[] design)
        {
            if (design.Length == 0)
            {
                return true;
            }

            foreach (var d in design.Where(t => !Possible.Contains(t)))
            {
                if (NotPossible.Contains(d)) { return false; }
                
                var isPossible = false;
                foreach (var t in Towels.
                             Where(t => t.Length <= d.Length).
                             Where(t => d.Contains(t)))
                {
                    if (t == d)
                    {
                        isPossible = true;
                        break;
                    }
                    var subTowels = d.Split(t, StringSplitOptions.RemoveEmptyEntries);
                    isPossible = IsPossible(subTowels);
                    if (isPossible) break;
                }

                if (isPossible)
                {
                    Possible.Add(d);
                }
                else
                {
                    NotPossible.Add(d);
                    return false;
                }
            }

            return true;
        }
    }

    public static Onsen Load(string[] data)
    {
        var towels = new Onsen(data[0], data.Skip(1).Where(d => !string.IsNullOrWhiteSpace(d)).ToList());
        return towels;
    }

    public static int PossibleDesigns(Onsen onsen)
    {
        var n = onsen.Designs.Count(t => onsen.IsPossible([t]));
        return n;
    }
}