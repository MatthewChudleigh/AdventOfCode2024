// List safe [[Level...]...]
// Safe if BOTH:
// - Levels are ALL increasing or ALL decreasing
// - Adjacent levels A,B: 1 <= abs(A-B) <= 3
// Tolerate a single bad level
// Expected Answer: 426

var dataPath = "/workspaces/AdventOfCode2024/data/A02/A02.1.txt";

var safeCount = 0;
var reports = File.ReadAllLines(dataPath).Select(line => line.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(Int32.Parse).ToList());

foreach (var levels in reports)
{
    if (A02.IsSafe(levels))
    {
        safeCount++;
    }
}

Console.WriteLine(safeCount);

public static class A02
{
    public static bool IsSafe(List<int> levels, bool tolerant = true)
    {
        // Create all possible combinations
        // Brute force to get a correct answer; optimse after
        var allLevels = new List<List<int>>();
        allLevels.Add(levels);
        for (var i = 0; i < levels.Count; ++i)
        {
            var level = levels.ToList();
            level.RemoveAt(i);
            if (level.Count == 0) continue;
            if (level.Count == 1) return true;
            allLevels.Add(level);
        }

        foreach (var level in allLevels)
        {
            var isSafe = true;
            var shouldBeIncreasing = level[0] < level[1];
            for (var i = 0; i < level.Count - 1; ++i)
            {
                if (!checkOk(shouldBeIncreasing, level[i], level[i + 1]))
                {   // Break out on the first bad level and proceed to the next combination
                    isSafe = false;
                    break;
                }
            }

            // If any of the possible combinations is safe, then the levels are safe
            if (isSafe) return true;
        }

        return false;
    }

    static bool checkOk(bool shouldBeIncreasing, int a, int b)
    {
        var delta = Math.Abs(a - b);
        var deltaOk = 1 <= delta && delta <= 3;
        var increasingOk = (a < b) == shouldBeIncreasing;

        return deltaOk && increasingOk;
    }
}