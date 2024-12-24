/*
   SN = (SN xor (SN * 2^6)) & (2^24 - 1)
   SN = (SN xor (SN * 2^-5)) & (2^24 - 1)
   SN = (SN xor (SN * 2^11)) & (2^24 - 1)

   Calculate the result of multiplying the secret number by 64. Then, mix this result into the secret number. 
   Finally, prune the secret number.
   
   Calculate the result of dividing the secret number by 32. Round the result down to the nearest integer. 
   Then, mix this result into the secret number.
   Finally, prune the secret number.
   
   Calculate the result of multiplying the secret number by 2048.
   Then, mix this result into the secret number.
   Finally, prune the secret number.

    (((SN xor (SN * 2^6)) & (2^24 - 1) xor ((SN xor (SN * 2^6)) & (2^24 - 1) * 2^-5)) & (2^24 - 1) xor (((SN xor (SN * 2^6)) & (2^24 - 1) xor ((SN xor (SN * 2^6)) & (2^24 - 1) * 2^-5)) & (2^24 - 1) * 2^11)) & (2^24 - 1)
 */

using System.Numerics;

var baseDir = Environment.GetEnvironmentVariable("AOC_BaseDir");
var seeds = File.ReadAllLines(Path.Combine(baseDir!, "A22.data.txt"));

Console.WriteLine(seeds.Select(BigInteger.Parse).Sum(s => Solution.Calculate(s, 2000)));

public static class Solution
{
    public static BigInteger Mask = (1L << 24) - 1;

    public static BigInteger Calculate(BigInteger n, int m)
    {
        for (var i = 0; i < m; ++i)
        {
            n = Calculate(n);
        }

        return n;
    }
    
    public static BigInteger Calculate(BigInteger n)
    {
        n = (n ^ (n << 6)) & Mask;
        n = (n ^ (n >> 5)) & Mask;
        n = (n ^ (n << 11)) & Mask;
        return n;
    }
    
    public static BigInteger Sum(this IEnumerable<BigInteger> numbers, Func<BigInteger, BigInteger> fn)
    {
        var s = new BigInteger(0);
        foreach (var n in numbers.Select(fn))
        {
            s += n;
        }

        return s;
    }
}