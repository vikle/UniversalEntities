namespace UniversalEntities
{
    public interface IFragment
    {
    };
    
    public interface IComponent : IFragment
    {
    };
    
    public interface IResettableComponent : IComponent
    {
        void OnReset();
    };
    
    public interface IUnmanagedComponent : IComponent
    {
    };
    
    public interface IEvent : IFragment
    {
    };
    
    public interface IPromise : IFragment
    {
        EPromiseState State { get; set; }
    };
}
