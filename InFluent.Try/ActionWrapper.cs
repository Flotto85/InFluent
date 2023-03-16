namespace InFluent.Try;

public class ActionWrapper
{
    private readonly Action _action;
    //private Action<Exception> _executeHandler;
    //private Type _exceptionType;
    private List<(Type, Action<Exception>)> _executeHandlers;

    public ActionWrapper(Action action)
    {
        _action = action;
        _executeHandlers = new List<(Type, Action<Exception>)>();
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
    {
        _executeHandlers.Single(x=>x.Item1 == exceptionType).Item2(e);    
    }

    internal bool HasExceptionType(Type exceptionType)
        => _executeHandlers.Count(x => x.Item1 == exceptionType) > 0;

    internal IEnumerable<Type> ExceptionTypes
        => _executeHandlers.Select(x => x.Item1);

    internal Action Action => _action;
}
