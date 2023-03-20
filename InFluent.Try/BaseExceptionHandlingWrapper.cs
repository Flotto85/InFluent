namespace InFluent.Try;

public abstract class BaseExceptionHandlingWrapper
{
    private readonly List<(Type, Action<Exception>)> _executeHandlers;

    internal protected BaseExceptionHandlingWrapper()
    {
        _executeHandlers = new();
    }

    internal void AddExceptionHandler<TException>(Action<TException> handler)
        where TException : Exception
    {
        _executeHandlers.Add((typeof(TException), (Exception e) =>
        {
            if (e is TException ex)
            {
                handler?.Invoke(ex);
            }
        }
        ));
    }

    internal void ExecuteHandler(Type exceptionType, Exception e)
        => _executeHandlers.First(x => x.Item1.IsAssignableFrom(exceptionType)).Item2(e);


    internal bool HasExceptionType(Type exceptionType)
        => _executeHandlers.Any(x => x.Item1.IsAssignableFrom(exceptionType));

    internal IEnumerable<Type> ExceptionTypes
        => _executeHandlers.Select(x => x.Item1);
}
