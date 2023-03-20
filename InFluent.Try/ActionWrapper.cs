namespace InFluent.Try;

public sealed class ActionWrapper : BaseExceptionHandlingWrapper
{
    private readonly Action _action;

    public ActionWrapper(Action action) : base()
    {
        _action = action;
    }

    internal Action Action => _action;
}