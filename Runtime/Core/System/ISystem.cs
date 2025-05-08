namespace UniversalEntities
{
    public interface ISystem
    {
    };
    
    public interface IStartSystem : ISystem
    {
        void OnStart(Context context);
    };
    
    public interface IFixedUpdateSystem : ISystem
    {
        void OnFixedUpdate(Context context);
    };
    
    public interface IUpdateSystem : ISystem
    {
        void OnUpdate(Context context);
    };
    
    public interface ILateUpdateSystem : ISystem
    {
        void OnLateUpdate(Context context);
    };
    
    public interface IEntityInitializeSystem : ISystem
    {
        void OnAfterEntityCreated(Context context, Entity entity);
    };
    
    public interface IEntityTerminateSystem: ISystem
    {
        void OnBeforeEntityDestroyed(Context context, Entity entity);
    };
}
