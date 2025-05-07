namespace UniversalEntities
{
    internal static class FragmentIndexCounter
    {
        internal static int count;
    };
    
    internal static class FragmentTypeIndex<T> where T : IFragment
    {
        internal static readonly int Index;

        static FragmentTypeIndex()
        {
            Index = FragmentIndexCounter.count++;
        }
    };
}
