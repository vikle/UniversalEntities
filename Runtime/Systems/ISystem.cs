namespace UniversalEntities
{
    public interface ISystem
    {
    };
    
    public interface IStartSystem : ISystem
    {
        void OnStart(IContext context);
    };
    
    public interface IFixedUpdateSystem : ISystem
    {
        void OnFixedUpdate(IContext context);
    };
    
    public interface IUpdateSystem : ISystem
    {
        void OnUpdate(IContext context);
    };
    
    public interface ILateUpdateSystem : ISystem
    {
        void OnLateUpdate(IContext context);
    };
    
    public interface IEntityInitializeSystem : ISystem
    {
        void OnAfterEntityCreated(IContext context, IEntity entity);
    };
    
    public interface IEntityTerminateSystem: ISystem
    {
        void OnBeforeEntityDestroyed(IContext context, IEntity entity);
    };
}
