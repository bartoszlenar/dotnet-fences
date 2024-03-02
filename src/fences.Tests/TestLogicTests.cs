namespace fences.Tests;

public class TestLogicTests
{
    [Fact(DisplayName = "XXX IsEven should return true for even numbers")]

    public void IsEvent()
    {
        var logic = new TestLogic();
        Assert.True(logic.IsEven(2));
        Assert.True(logic.IsEven(4));
        Assert.True(logic.IsEven(6));
    }
}