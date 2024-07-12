namespace UniversalEntities
{
    public interface IContext
    {
        IEntity CreateEntity<T>() where T : class, IEntity, new();
        void DestroyEntity(IEntity entity);
        void AddEntity(IEntity entity);
        void RemoveEntity(IEntity entity);
        ContextEnumerator GetEnumerator();
    };
}
