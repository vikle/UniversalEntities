namespace UniversalEntities
{
    public sealed class FragmentFactory : ObjectPool<IFragment>
    {
        public static T GetInstance<T>() where T : class, IFragment, new()
        {
            return GetInstanceInternal<T>();
        }
        
        public static void Release<T>(T instance) where T : class, IFragment
        {
            ReleaseInternal(instance);
        }
    };
}
