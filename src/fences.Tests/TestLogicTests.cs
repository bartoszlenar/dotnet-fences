using FluentAssertions;

namespace fences.Tests;

public class TestLogicTests
{
    [Fact(DisplayName = "IsEven should return true for even numbers")]

    public void IsEven()
    {
        var logic = new TestLogic();
        logic.IsEven(2).Should().BeTrue();
        logic.IsEven(4).Should().BeTrue();
        logic.IsEven(6).Should().BeTrue();
    }
}