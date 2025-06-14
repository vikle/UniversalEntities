namespace UniversalEntities
{
    internal static class FragmentIndexCounter
    {
        internal static int Count;

        internal static int Increment() { return Count++; }
    };
    
    internal static class FragmentTypeIndex<T> where T : IFragment
    {
        internal static readonly int Index;

        static FragmentTypeIndex() { Index = FragmentIndexCounter.Increment(); }
    };
}
