using InFluent.Try;

namespace InFluent;

public static partial class Tools
{
    public static ActionWrapper Try(Action action)
    {
        return new ActionWrapper(action);
    }

    public static FunctionWrapper<T> Try<T>(Func<T> func)
    {
        return new FunctionWrapper<T>(func);
    } 
}