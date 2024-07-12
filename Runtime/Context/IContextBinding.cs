namespace UniversalEntities
{
    public interface IContextBinding
    {
        IContextBinding BindEvent<T>() where T : class, IEvent;
        IContextBinding BindPromise<T>() where T : class, IPromise;
        IContextBinding BindSystem<T>() where T : class, ISystem, new();
        IContextBinding Inject<T>(T injection) where T : class;
    };
}
