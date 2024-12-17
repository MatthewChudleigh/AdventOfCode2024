namespace A17;

public class Computer
{
    public int RegA { get; set; }
    public int RegB { get; set; }
    public int RegC { get; set; }

    public int Pointer { get; set; }
    public List<int> Instructions { get; set; } = new();
    public List<int> Output { get; set; } = new();

    public Dictionary<int, IOp> OpCodes { get; } = new Dictionary<int, IOp>()
    {
        { 0, new OpAdv() },
        { 1, new OpBxl() },
        { 2, new OpBst() },
        { 3, new OpJnz() },
        { 4, new OpBxc() },
        { 5, new OpOut() },
        { 6, new OpBdv() },
        { 7, new OpCdv() }
    };

    public int Combo(int operand)
    {
        if (operand <= 3) return operand;
        if (operand == 4) return RegA;
        if (operand == 5) return RegB;
        if (operand == 6) return RegC;
        throw new InvalidOperationException();
    }

    public void Calculate()
    {
        while (Pointer + 1 < Instructions.Count)
        {
            var op = OpCodes[Instructions[Pointer]];
            var operand = Instructions[Pointer + 1];
            op.Operate(this, operand);
        }
    }
}

public interface IOp
{
    void Operate(Computer computer, int operand);
}

public class OpXdv
{
    public static int Operate(Computer computer, int operand)
    {
        operand = computer.Combo(operand);
        var num = computer.RegA;
        var den = 0b1 << operand;

        computer.Pointer += 2;
        return (num / den);
    }
}

public class OpAdv : IOp
{
    public void Operate(Computer computer, int operand)
    {
        computer.RegA = OpXdv.Operate(computer, operand);
    }
}

public class OpBdv : IOp
{
    public void Operate(Computer computer, int operand)
    {
        computer.RegB = OpXdv.Operate(computer, operand);
    }
}

public class OpCdv : IOp
{
    public void Operate(Computer computer, int operand)
    {
        computer.RegC = OpXdv.Operate(computer, operand);
    }
}

public class OpBxl : IOp
{
    public void Operate(Computer computer, int operand)
    {
        var xor = computer.RegB ^ operand;
        computer.RegB = xor;
        computer.Pointer += 2;
    }
}

public class OpBst : IOp
{
    public void Operate(Computer computer, int operand)
    {
        operand = computer.Combo(operand);
        var mod = operand % 8;
        computer.RegB = mod;
        computer.Pointer += 2;
    }
}

public class OpJnz : IOp
{
    public void Operate(Computer computer, int operand)
    {
        if (computer.RegA != 0)
        {
            computer.Pointer = operand;
        }
        else
        {
            computer.Pointer += 2;
        }
    }
}

public class OpBxc : IOp
{
    public void Operate(Computer computer, int operand)
    {
        var xor = computer.RegB ^ computer.RegC;
        computer.RegB = xor;
        computer.Pointer += 2;
    }
}

public class OpOut : IOp
{
    public void Operate(Computer computer, int operand)
    {
        operand = computer.Combo(operand);
        var mod = operand % 8;
        computer.Output.Add(mod);
        computer.Pointer += 2;
    }
}

public static class Solution
{
    public static Computer Load(string[] data)
    {
        var regA = int.Parse(data[0].Split(":")[1]);
        var regB = int.Parse(data[1].Split(":")[1]);
        var regC = int.Parse(data[2].Split(":")[1]);
        var instructions = data[4].Split(":")[1].Split(",").Select(Int32.Parse).ToList();

        return new Computer()
        {
            Instructions = instructions,
            RegA = regA,
            RegB = regB,
            RegC = regC,
            Pointer = 0
        };
    }
}