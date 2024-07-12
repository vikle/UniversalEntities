namespace UniversalEntities
{
    public interface IEntityTerminateSystem: ISystem
    {
        void OnBeforeEntityDestroyed(IContext context, IEntity entity);
    };
}
