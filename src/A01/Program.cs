
A01.Run("/workspaces/AdventOfCode2024/data/A01/A01.txt");

public static class A01 {
    /*
    01

    Example:

    3   4
    4   3
    2   5
    1   3
    3   9
    3   3

    - Pair up the numbers and measure how far apart they are
    - the smallest number in the left list with the smallest number in the right list
    - the second-smallest left number with the second-smallest right number
    - Within each pair, figure out how far apart the two numbers are
    - Find the total distance between the left list and the right list
    - add up the distances between all of the pairs you found

    For example:
    - if you pair up a 3 from the left list with a 7 from the right list, the distance apart is 4;
    - if you pair up a 9 with a 3, the distance apart is 6.
    - In the example above, this is 2 + 1 + 0 + 1 + 2 + 5, a total distance of 11

    ******

    02

    - Figure out exactly how often each number from the left list appears in the right list
      Calculate a total similarity score by adding up each number in the left list,
      after multiplying it by the number of times that number appears in the right list.

    */

    public static void Run(string dataPath) {

        var lhs = new List<int>();
        var rhs = new List<int>();
        var counts = new Dictionary<int, int>();

        var fields = File
            .ReadAllLines(dataPath)
            .Select(line => line.Split(" ", StringSplitOptions.RemoveEmptyEntries));

        foreach (var fieldPair in fields) {
            var lhsValue = Int32.Parse(fieldPair[0]);
            var rhsValue = Int32.Parse(fieldPair[1]);

            lhs.Add(lhsValue);
            rhs.Add(rhsValue);

            counts.TryGetValue(rhsValue, out var count);
            counts[rhsValue] = count + 1;
        }

        lhs.Sort();
        rhs.Sort();

        var sum = 0;
        var similarity = 0;

        for (var i = 0; i < lhs.Count; ++i) {
            sum += Math.Abs(rhs[i] - lhs[i]);

            counts.TryGetValue(lhs[i], out var count);
            similarity += lhs[i] * count;
        }

        Console.WriteLine($"Sum total differences: {sum}");
        Console.WriteLine($"Similarity score: {similarity}");
    }
}