namespace InFluent.Try.Tests;

using System;
using static InFluent.Tools;

public class ActionWrapperExtensionTests
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

        Try(action)
            .Catch<Exception>(e => { })
            .Finally(handler);
        handled.Should().BeTrue();

        void action() { }
        void handler() { handled = true; }
    }

    [Fact]
    public void CatchesAndHandlesExceptionWithMultipleExceptionHandlersDefined()
    {
        TestMultipleExceptionHandlers<
            ArgumentException,
            OverflowException,
            Exception,
            OverflowException>();
        TestMultipleExceptionHandlers<
            ArgumentException,
            Exception,
            OverflowException,
            ArgumentException>();
        TestMultipleExceptionHandlers<
            ArgumentException,
            Exception,
            OverflowException,
            ArgumentException>();
    }
    private void TestMultipleExceptionHandlers<T1, T2, T3, TExpected>()
        where T1 : Exception
        where T2 : Exception
        where T3 : Exception
        where TExpected : Exception, new()
    {
        Type? gotType = null;
        Type expectedType = typeof(TExpected);

        Try(action)
            .Catch<T1>(handler1)
            .Catch<T2>(handler2)
            .Catch<T3>(handler3)
            .Finally();

        gotType
            .Should()
            .Be(expectedType);

        void action() { throw new TExpected(); }
        void handler1(Exception e) { gotType = typeof(T1); }
        void handler2(Exception e) { gotType = typeof(T2); }
        void handler3(Exception e) { gotType = typeof(T3); }
    }
}
