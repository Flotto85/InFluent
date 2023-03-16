namespace InFluent.Try.Tests;

using System.Reflection.Metadata;
using static InFluent.Tools;

public class TryTests
{
    [Fact]
    public void CatchesAndHandlesException()
    {
        var expected = new ArgumentNullException();
        ArgumentNullException? got = null;

        Try(action)
            .Catch<ArgumentNullException>(handler)
            .Finally();
        got.Should().Be(expected);

        void action() { throw expected; }
        void handler(ArgumentNullException e) { got = e; }
    }

    [Fact]
    public void DoesNotCatchAndHandleDifferentException()
    {
        var expected = new ArgumentNullException();
        Exception? got = null;

        Action test = () => Try(action)
        .Catch<ArgumentNullException>(handler)
        .Finally();
        test.Should().Throw<OverflowException>();

        void action() { throw new OverflowException(); }
        void handler(Exception e) { got = e; }
    }

    [Fact]
    public void ExecutesFinallyHandlerDirectlyAfterTry()
    {
        bool handled = false;

        Try(action).Finally(handler);
        handled.Should().BeTrue();

        void action() { }
        void handler() { handled = true; }
    }

    [Fact]
    public void ExecutesActionWhenFinallyWithHandlerIsCalledDirectlyAfterTry()
    {
        bool executed = false;

        Try(action).Finally(handler);
        executed.Should().BeTrue();

        void action() { executed = true; }
        void handler() { }
    }

    [Fact]
    public void ExecutesActionWhenFinallyWithoutHandlerIsCalledDirectlyAfterTry()
    {
        bool executed = false;

        Try(action).Finally();
        executed.Should().BeTrue();

        void action() { executed = true; }
    }

    [Fact]
    public void ExecutesFinallyHandlerAfterCatch()
    {
        bool handled = false;

        Try(action).Catch<Exception>(e => { }).Finally(handler);
        handled.Should().BeTrue();

        void action() { }
        void handler() { handled = true; }
    }

    [Fact]
    public void CatchesAndHandlesExceptionWithMultipleExceptionHandlersDefined()
    {
        Type? gotType = null;
        Type expectedType = typeof(OverflowException);

        Try(action)
            .Catch<ArgumentNullException>(handler)
            .Catch<OverflowException>(handler)
            .Catch<Exception>(handler)
            .Finally();

        void action() { throw new OverflowException(); }
        void handler(Exception e) { gotType = e.GetType(); }

        gotType.Should().Be(expectedType);
    }
}