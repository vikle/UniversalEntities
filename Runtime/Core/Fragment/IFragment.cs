namespace UniversalEntities
{
    public interface IFragment
    {
    };
    
    public interface IComponent : IFragment
    {
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
