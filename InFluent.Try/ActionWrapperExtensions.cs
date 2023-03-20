namespace InFluent.Try;

public static class ActionWrapperExtensions
{
    public static ActionWrapper Catch<TException>(
        this ActionWrapper wrapper,
        Action<TException> handler)
        where TException : Exception
    {
        wrapper.AddExceptionHandler(handler);
        return wrapper;
    }

    public static void Finally(this ActionWrapper wrapper, Action handler)
    {
        try
        {
            wrapper.Action?.Invoke();
        }
        catch(Exception e) when (wrapper.HasExceptionType(e.GetType()))
        {
            wrapper.ExecuteHandler(e.GetType(), e);
        }
        finally
        {
            handler?.Invoke();
        }
    }

    public static void Finally(this ActionWrapper wrapper)
    {
#pragma warning disable CS8625 // Ein NULL-Literal kann nicht in einen Non-Nullable-Verweistyp konvertiert werden.
        wrapper.Finally(null);
#pragma warning restore CS8625 // Ein NULL-Literal kann nicht in einen Non-Nullable-Verweistyp konvertiert werden.
    }
}
