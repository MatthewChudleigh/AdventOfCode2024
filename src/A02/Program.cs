// List safe [[Level...]...]
// Safe if BOTH:
// - Levels are ALL increasing or ALL decreasing
// - Adjacent levels A,B: 1 <= abs(A-B) <= 3

var dataPath = "/workspaces/AdventOfCode2024/data/A02/A02.1.txt";

var safeCount = 0;
var reports = File.ReadAllLines(dataPath).Select(line => line.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(Int32.Parse).ToArray());

foreach (var levels in reports)
{
    var safe = true;

    if (levels.Length > 1)
    {
        var allIncreasing = levels[0] < levels[1];
        for (var i = 0; i < levels.Length - 1; ++i)
        {
            var delta = Math.Abs(levels[i] - levels[i + 1]);
            var increasing = levels[i] < levels[i + 1];
            safe = (allIncreasing == increasing && 1 <= delta && delta <= 3);
            if (!safe) break;
        }
    }

    if (safe)
    {
        safeCount++;
    }
}

Console.WriteLine(safeCount);