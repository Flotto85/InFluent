namespace InFluent.Try;

public class FunctionWrapper<T>
{
    private readonly Func<T> _func;
    private readonly List<(Type, Func<Exception, T>)> _executeHandlers;

    public FunctionWrapper(Func<T> func)
    {
        _func = func;
        _executeHandlers = new();
    }

    internal void AddExceptionHandler<TException>(Func<TException, T> handler)
        where TException : Exception
    {
        Func<Exception, T> exceptionHandler =
            (Exception ex) => handler.Invoke((TException)ex);

        _executeHandlers.Add((typeof(TException), exceptionHandler));
    }

    internal T ExecuteHandler(Type exceptionType, Exception e)
        => _executeHandlers.First(x => x.Item1.IsAssignableFrom(exceptionType)).Item2(e);


    internal bool HasExceptionType(Type exceptionType)
        => _executeHandlers.Any(x => x.Item1.IsAssignableFrom(exceptionType));

    internal Func<T> Func => _func;
}
