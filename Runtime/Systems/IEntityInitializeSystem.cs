namespace UniversalEntities
{
    public interface IEntityInitializeSystem : ISystem
    {
        void OnAfterEntityCreated(IContext context, IEntity entity);
    };
}
