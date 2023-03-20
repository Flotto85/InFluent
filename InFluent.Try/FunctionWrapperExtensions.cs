namespace InFluent.Try;

public static class FunctionWrapperExtensions
{
    public static FunctionWrapper<T> Catch<T, TException>(
        this FunctionWrapper<T> wrapper,
        Func<TException, T> handler)
        where TException : Exception
    {
        wrapper.AddExceptionHandler(handler);
        return wrapper;
    }

    public static T Finally<T>(this FunctionWrapper<T> wrapper, Action handler)
    {
        try
        {
            return wrapper.Func.Invoke();
        }
        catch (Exception e) when (wrapper.HasExceptionType(e.GetType()))
        {
            return wrapper.ExecuteHandler(e.GetType(), e);
        }
        finally
        {
            handler?.Invoke();
        }
    }

    public static T Finally<T>(this FunctionWrapper<T> wrapper)
    {
#pragma warning disable CS8625 // Ein NULL-Literal kann nicht in einen Non-Nullable-Verweistyp konvertiert werden.
        return wrapper.Finally(null);
#pragma warning restore CS8625 // Ein NULL-Literal kann nicht in einen Non-Nullable-Verweistyp konvertiert werden.
    }
}
