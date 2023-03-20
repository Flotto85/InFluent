namespace InFluent.Try.Tests;

using static InFluent.Tools;

public class FunctionWrapperExtensionTests
{
    [Fact]
    public void FinallyReturnsFunctionsReturnValue()
    {
        const int a = 1;
        const int b = 2;
        int expected = func(a, b);

        int got = Try(() => func(a, b)).Finally();

        got.Should().Be(expected);

        static int func(int x, int y) => x + y;
    }
}
