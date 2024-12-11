namespace A11;

public static class Solution
{
    public class Stone
    { 
        public long Value { get; set; }
        public long Count { get; set; }
    }
    
    //   A        B         2
    //  A   A   B   B       4
    // A B A B C C C C      8
    
    public static long Solve(string dataPath, int iter)
    {
        var stones = File.ReadAllText(dataPath).Split(" ", StringSplitOptions.RemoveEmptyEntries)
            .Select(long.Parse)
            .Select(n => new Stone() { Count = 1, Value = n })
            .ToList();

        stones = Generate(stones, iter);
        
        return stones.Sum(s => s.Count);
    }

    public static List<Stone> Generate(List<Stone> stones, int iter)
    {
        while (iter > 0)
        {
            stones = stones.SelectMany(s =>
            {
                if (s.Value == 0)
                {
                    return new List<Stone>()
                    {
                        new Stone() { Count = s.Count, Value = 1 }
                    };
                }
                
                var str = $"{s.Value}";
                if (str.Length % 2 == 0)
                {
                    var lhs = str.Substring(0, str.Length / 2);
                    var rhs = str.Substring(str.Length / 2);
                    return
                    [
                        new Stone() { Count = s.Count, Value = long.Parse(lhs) },
                        new Stone() { Count = s.Count, Value = long.Parse(rhs) }
                    ];
                }
                else
                {
                    return [new Stone() { Count = s.Count, Value = s.Value * 2024 }];
                }
            }).GroupBy(s => s.Value).Select(group => new Stone()
            {
                Value = group.Key,
                Count = group.Sum(s => s.Count)
            }).ToList();
            iter--;
        }

        return stones;
    }
}