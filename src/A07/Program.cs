/*
- equation per line
    [Test Value]: [N1, N2...]
- calculate the sum of the equations that can be produced using operators (+) and (*)
- operators are evaluated left-to-right, not according to precedence rules
- numbers cannot be rearranged
*/

var dataPath = "/workspaces/AdventOfCode2024/data/A07/A07.1.txt";

var total = A07.Calculate(dataPath);
Console.WriteLine(total);

static class A07 {
    class Equation {
        public long TestValue {get;set;}
        public required List<long> Terms {get;set;}
    }
    
    public static long Calculate(string dataPath, int opCount = 3) {
        var equations = new List<Equation>();

        foreach (var line in File.ReadAllLines(dataPath)) {
            var parts = line.Split(":", StringSplitOptions.RemoveEmptyEntries);
            var equation = new Equation() {
                TestValue = Int64.Parse(parts[0]),
                Terms = parts[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(Int64.Parse).ToList()
            };
            equations.Add(equation);
        }

        long result = 0;
        foreach (var equation in equations) {

            var termIter = 0;
            var termsCheck = Math.Pow(opCount, equation.Terms.Count);
            var operators = Enumerable.Repeat(0, equation.Terms.Count - 1).ToList();
            while (termIter < termsCheck) {
                
                var total = equation.Terms[0];
                for (var i = 0; i < equation.Terms.Count - 1; ++i) {
                    var rhs = equation.Terms[i + 1];
                    total = (operators[i]%opCount) switch { 0 => (total * rhs), 1 => (total + rhs), 2 => Int64.Parse($"{total}{rhs}"), _ => 0 };
                }

                if (total == equation.TestValue) {
                    result += equation.TestValue;
                    break;
                }
                
                for (var i = 0; i < operators.Count; ++i) {
                    operators[i]++;
                    if (operators[i]%opCount != 0) {
                        break;
                    }
                }

                termIter++;
            }
        }

        return result;
    }
}