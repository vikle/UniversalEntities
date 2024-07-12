namespace UniversalEntities
{
    public sealed class EntityPool : ObjectPool<IEntity>
    {
        public static T Get<T>() where T : class, IEntity, new()
        {
            return GetInternal<T>();
        }
        
        public static void Release<T>(T instance) where T : class, IEntity
        {
            ReleaseInternal(instance);
        }
    };
}
