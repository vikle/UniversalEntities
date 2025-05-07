namespace UniversalEntities
{
    internal sealed class FragmentPool : ObjectPool<IFragment>
    {
        public static T Get<T>() where T : class, IFragment, new()
        {
            return GetInternal<T>();
        }
        
        public static void Release<T>(T instance) where T : class, IFragment
        {
            ReleaseInternal(instance);
        }
    };
}
