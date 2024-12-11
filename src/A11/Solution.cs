using System.Numerics;

namespace A11;

public static class BigIntExt
{
    public static BigInteger Sum(this IEnumerable<BigInteger> bigInts)
    {
        BigInteger sum = 0;
        foreach (var i in bigInts)
        {
            sum += i;
        }

        return sum;
    }
}

public static class Solution
{
    public record Stone(BigInteger Value, BigInteger Count);
    
    public static BigInteger Solve(string dataPath, int iter)
    {
        var stones = File.ReadAllText(dataPath).Split(" ", StringSplitOptions.RemoveEmptyEntries)
            .Select(BigInteger.Parse)
            .Select(n => new Stone(n, 1))
            .ToList();

        stones = Generate(stones, iter);
        
        return stones.Select(s => s.Count).Sum();
    }

    public static List<Stone> Generate(List<Stone> stones, int iter)
    {
        while (iter > 0)
        {
            stones = stones.SelectMany(SplitStone)
                .GroupBy(s => s.Value)
                .Select(group => new Stone(group.Key, group.Select(s => s.Count).Sum()))
                .ToList();
            iter--;
        }

        return stones;
    }

    private static IEnumerable<Stone> SplitStone(Stone s)
    {
        if (s.Value == 0)
        {
            yield return s with { Value = 1 };
            yield break;
        }
            
        var str = $"{s.Value}";
        if (str.Length % 2 != 0) {
            yield return s with { Value = s.Value * 2024 };
        } else {
            var lhs = str[..(str.Length / 2)];
            var rhs = str[(str.Length / 2)..];
            yield return s with { Value = BigInteger.Parse(lhs) };
            yield return s with { Value = BigInteger.Parse(rhs) };
        }
    }
}
