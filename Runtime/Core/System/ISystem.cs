namespace UniversalEntities
{
    public interface ISystem
    {
    };
    
    public interface IStartSystem : ISystem
    {
        void OnStart(Pipeline pipeline);
    };
    
    public interface IFixedUpdateSystem : ISystem
    {
        void OnFixedUpdate(Pipeline pipeline);
    };
    
    public interface IUpdateSystem : ISystem
    {
        void OnUpdate(Pipeline pipeline);
    };
    
    public interface ILateUpdateSystem : ISystem
    {
        void OnLateUpdate(Pipeline pipeline);
    };
    
    public interface IEntityInitializeSystem : ISystem
    {
        void OnAfterEntityCreated(Pipeline pipeline, Entity entity);
    };
    
    public interface IEntityTerminateSystem: ISystem
    {
        void OnBeforeEntityDestroyed(Pipeline pipeline, Entity entity);
    };
}
