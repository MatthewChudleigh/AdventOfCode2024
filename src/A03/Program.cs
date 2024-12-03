using System.Text.RegularExpressions;

/*
- mul(X,Y)
  - X and Y are each 1-3 digit numbers
  - e.g., mul(44,46) -> 2024
- do()
  - enables future mul
- don't()
  - disables future mul
- mul enabled by default

- invalid characters to be ignored
    - Sequences like mul(4*, mul(6,9!, ?(12,34), or mul ( 2 , 4 ) do nothing.

xmul(2,4)%&mul[3,7]!@^do_not_mul(5,5)+mul(32,64]then(mul(11,8)mul(8,5))

mul(2,4) + mul(5,5) + mul(11,8) + mul(8,5)

(2*4 + 5*5 + 11*8 + 8*5) = 161

- Sum total sum of all sequences
*/

var dataPath = "/workspaces/AdventOfCode2024/data/A03/A03.1.txt";

var sumTotal = 0;
var sequences = File.ReadAllLines(dataPath);

var doing = true;
foreach (var sequence in sequences)
{
    var sum = 0;
    (sum, doing) = A03.Evaluate(doing, sequence);
    sumTotal += sum;
}

Console.WriteLine(sumTotal);

static partial class A03
{
    [GeneratedRegex(@"(?<do>do\(\))|(?<dont>don't\(\))|mul\((?<lhs>\d{1,3}),(?<rhs>\d{1,3})\)", RegexOptions.Compiled)]
    private static partial Regex ReMul();
    public static (int sum, bool doing) Evaluate(bool doing, string sequence)
    {
        var sum = 0;
        foreach (Match match in ReMul().Matches(sequence))
        {
            if (match.Groups["do"].Success)
            {
                doing = true;
            }
            else if (match.Groups["dont"].Success)
            {
                doing = false;
            }
            else if (doing)
            {
                var lhs = Int32.Parse(match.Groups["lhs"].Value);
                var rhs = Int32.Parse(match.Groups["rhs"].Value);
                sum += lhs * rhs;
            }
        }
        return (sum, doing);
    }
}