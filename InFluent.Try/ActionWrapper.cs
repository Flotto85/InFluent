namespace InFluent.Try;

public class ActionWrapper
{
    private readonly Action _action;
    private Action<Exception> _executeHandler;
    private Type _exceptionType;

    public ActionWrapper(Action action)
    {
        _action = action;
    }

    internal void AddExceptionHandler<TException>(Action<TException> handler)
        where TException : Exception
    {
        _exceptionType = typeof(TException);
        _executeHandler = (Exception e) =>
        {
            var ex = e as TException;
            if (ex != null)
            {
                handler?.Invoke(ex);
            }
        };
    }

    internal void ExecuteHandler(Exception e)
    {
        _executeHandler(e);
    }
    internal Type ExceptionType => _exceptionType;

    internal Action Action => _action;
}
