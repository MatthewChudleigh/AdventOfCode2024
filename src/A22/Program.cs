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
 */

var baseDir = Environment.GetEnvironmentVariable("AOC_BaseDir");
var seeds = File.ReadAllLines(Path.Combine(baseDir!, "A22.data.txt"));

Console.WriteLine(seeds.Select(long.Parse).Sum(s => Solution.Calculate(s, 2000)));
var bananas = Solution.BaNaNaS.Max(b => b.Value);
Console.WriteLine(bananas);

public static class Solution
{
    public static long Mask = (1L << 24) - 1;
    public record Seq(long A, long B, long C, long D);
    public static Dictionary<Seq, long> BaNaNaS = new();

    public static long Calculate(long n, int m)
    {
        var sequenced = new HashSet<Seq>();
        List<long> sx = [0, 0, 0, 0];
        
        for (var i = 0; i < m; ++i)
        {
            var n1 = Calculate(n);
            sx[i % 4] = (n1 % 10) - (n % 10);
            n = n1;
            
            if (i < 3) continue;
            
            var sequence = new Seq(sx[(i-3)%4], sx[(i-2)%4], sx[(i-1)%4], sx[i%4]);
            if (!sequenced.Add(sequence)) continue;
            
            BaNaNaS.TryGetValue(sequence, out var b);
            BaNaNaS[sequence] = b + (n1 % 10);
        }

        return n;
    }
    
    public static long Calculate(long n)
    {
        n = (n ^ (n << 6)) & Mask;
        n = (n ^ (n >> 5)) & Mask;
        n = (n ^ (n << 11)) & Mask;
        return n;
    }
}