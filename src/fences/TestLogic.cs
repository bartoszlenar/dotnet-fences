class TestLogic
{
#pragma warning disable CA1822 // Mark members as static
    public bool IsEven(int number)
#pragma warning restore CA1822 // Mark members as static
    {
        return number % 2 == 0;
    }
}