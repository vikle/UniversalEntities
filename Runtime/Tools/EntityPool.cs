namespace UniversalEntities
{
    internal sealed class EntityPool : ObjectPool<Entity>
    {
        public static Entity Get()
        {
            return GetInternal<Entity>();
        }
        
        public static void Release(Entity instance)
        {
            ReleaseInternal(instance);
        }
    };
}
