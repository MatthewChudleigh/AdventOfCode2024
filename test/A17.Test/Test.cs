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
    public void TestComputer(int a, int b, int c, string str, int expectedA, int expectedB, int expectedC, string expectedOutput)
    {
        var computer = new Computer()
        {
            RegA = a,
            RegB = b,
            RegC = c,
            Instructions = str.Split(",").Select(Int32.Parse).ToList()
        };
        
        computer.Calculate();
        
        Assert.Equal(expectedA, computer.RegA);
        Assert.Equal(expectedB, computer.RegB);
        Assert.Equal(expectedC, computer.RegC);
        Assert.Equal(expectedOutput, string.Join(",",computer.Output));
    }
}
