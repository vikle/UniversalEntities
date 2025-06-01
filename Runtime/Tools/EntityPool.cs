namespace UniversalEntities
{
    internal sealed class EntityPool : ObjectPool<Entity>
    {
        internal static Entity Get()
        {
            return GetInternal<Entity>();
        }
        
        internal static void Release(Entity instance)
        {
            ReleaseInternal(instance);
        }
    };
}
