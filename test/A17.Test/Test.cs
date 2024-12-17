namespace A17;

public class Test
{
    [Theory]
    [InlineData(0,0,9,"2,6", 0,1,9, "")]
    [InlineData(0,29,0,"1,7", 0,26,0, "")]
    [InlineData(0,2024,43690,"4,0", 0,44354,43690, "")]
    [InlineData(10,0,0,"5,0,5,1,5,4", 10,0,0, "0,1,2")]
    [InlineData(2024,0,0,"0,1,5,4,3,0", 0,0,0, "4,2,5,6,7,7,7,7,3,1,0")]
    [InlineData(729,0,0,"0,1,5,4,3,0", 0,0,0, "4,6,3,5,6,3,5,2,1,0")]
    public void TestComputer(ulong a, ulong b, ulong c, string str, ulong expectedA, ulong expectedB, ulong expectedC, string expectedOutput)
    {
        var state = new Computer.StateData(a, b, c, 0);
        var instructions = str.Split(",").Select(ushort.Parse).ToList();
        var targetOutput = expectedOutput.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(ushort.Parse).ToList();
        var computer = new Computer(state, instructions, targetOutput);
        
        computer.Calculate();
        
        Assert.Equal(expectedA, computer.State.A);
        Assert.Equal(expectedB, computer.State.B);
        Assert.Equal(expectedC, computer.State.C);
        Assert.Equal(expectedOutput, string.Join(",",computer.Output));
    }
}
