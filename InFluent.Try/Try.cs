using InFluent.Try;

namespace InFluent;

public static partial class Tools
{
    public static ActionWrapper Try(Action action)
    {
        return new ActionWrapper(action);
    }
}