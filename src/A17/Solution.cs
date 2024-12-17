using System.Numerics;

namespace A17;

public class Computer(Computer.StateData state, List<ushort> instructions, List<ushort>? targetOutput)
{
    public record StateData(BigInteger A, BigInteger B, BigInteger C, BigInteger Pointer);

    public StateData State { get; private set; } = state;
    public IReadOnlyList<ushort> Instructions { get; } = instructions;
    public IReadOnlyList<ushort>? TargetOutput { get; } = targetOutput;
    public List<int> Output { get; } = [];

    public Dictionary<int, Func<Computer, BigInteger, StateData>> OpCodes { get; } = new()
    {
        { 0, (c, i) => c.State with { A = Operate(c, i), Pointer = c.State.Pointer + 2 } },
        { 1, (c, i) => c.State with { B = c.State.B ^ i, Pointer = c.State.Pointer + 2 } },
        { 2, (c, i) => c.State with { B = c.Combo(i) % 8, Pointer = c.State.Pointer + 2 } },
        { 3, (c, i) => c.State with { Pointer = c.State.A != 0 ? i : c.State.Pointer + 2 } },
        { 4, (c, _) => c.State with { B = c.State.B ^ c.State.C, Pointer = c.State.Pointer + 2 } },
        { 5, OpOutput },
        { 6, (c, i) => c.State with { B = Operate(c, i), Pointer = c.State.Pointer + 2 } },
        { 7, (c, i) => c.State with { C = Operate(c, i), Pointer = c.State.Pointer + 2 }}
    };
    
    public static StateData OpOutput(Computer computer, BigInteger operand)
    {
        computer.Output.Add((ushort)(computer.Combo(operand) % 8));
        var idx = computer.Output.Count - 1;
        if (computer.TargetOutput != null && (idx >= computer.TargetOutput.Count || computer.Output[idx] != computer.TargetOutput[idx]))
        {
            return computer.State with { Pointer = (ulong)computer.Instructions.Count + 1 };
        }
        return computer.State with { Pointer = computer.State.Pointer + 2 };
    }
    
    public static BigInteger Operate(Computer computer, BigInteger operand)
    {
        var op = computer.Combo(operand);
        var num = computer.State.A;
        var den = (ulong)0b1 << (ushort)op;

        return (num / den);
    }

    public BigInteger Combo(BigInteger operand)
    {
        if (operand <= 3) return operand;
        if (operand == 4) return State.A;
        if (operand == 5) return State.B;
        if (operand == 6) return State.C;
        throw new InvalidOperationException();
    }

    public void Calculate()
    {
        while (State.Pointer + 1 < (ulong)Instructions.Count)
        {
            var op = OpCodes[Instructions[(ushort)State.Pointer]];
            var operand = Instructions[(ushort)State.Pointer + 1];
            State = op(this, operand);
        }
    }
}

public static class Solution
{
    public static Computer Load(string[] data, List<ushort>? targetOutput)
    {
        var regA = ulong.Parse(data[0].Split(":")[1]);
        var regB = ulong.Parse(data[1].Split(":")[1]);
        var regC = ulong.Parse(data[2].Split(":")[1]);
        var instructions = data[4].Split(":")[1].Split(",").Select(ushort.Parse).ToList();

        var state = new Computer.StateData(regA, regB, regC, 0);
        return new Computer(state, instructions, targetOutput);
    }

    public static BigInteger? Quine(IReadOnlyList<ushort> targetOutput)
    {
        return Quine(targetOutput, 0, 0, 0);
    }

    private static BigInteger? Quine(IReadOnlyList<ushort> targetOutput, int index, BigInteger output, BigInteger writeMask)
    {
        if (index == targetOutput.Count)
        {
            return output;
        }
        // a == A%8
        //  ((A >> (a xor 7)) xor a) % 8
        for (ushort i = 0; i <= 7; ++i)
        {
            var q = Quine(targetOutput, index, i, output, writeMask);
            if (q != null)
            {
                return q;
            }
        }

        return null;
    }

    private static BigInteger? Quine(IReadOnlyList<ushort> targetOutput, int index, ushort i, BigInteger output, BigInteger writeMask)
    {
        var target = (BigInteger)targetOutput[index];
        var inputMask = (BigInteger)(7) << (index * 3);
        var input = (BigInteger)(i) << (index * 3);

        if ( (output & inputMask & writeMask) != (input & writeMask))
        {   // Prevent overwrite with different values
            return null;
        }

        writeMask |= inputMask;

        var offset = index * 3 + (i ^ 7);
        var valMask = (BigInteger)(7) << offset;
        var val = (target ^ i) << offset;
        if ((output & valMask & writeMask) != (val & writeMask))
        {   // Prevent overwrite with different values
            return null;
        }
        
        writeMask |= valMask;
        return Quine(targetOutput, ++index, output | input | val, writeMask);
    }
}