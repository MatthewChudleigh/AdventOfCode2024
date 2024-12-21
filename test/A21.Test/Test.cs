namespace A21.Test;

public class Test
{
    [Theory]
    [InlineData("A0", "<")]
    [InlineData("A3", "^")]
    [InlineData("A2", "<^")]
    [InlineData("A8", "<^^^")]
    [InlineData("10", ">v")]
    [InlineData("4A", ">>vv")]
    public void TestKeypad(string code, string expectedMoves)
    {
        var keypad = new Solution.Keypad();
        keypad.Init();
        var moves = keypad.GetMoves(code);
        Assert.Equal(expectedMoves, moves);
    }
}