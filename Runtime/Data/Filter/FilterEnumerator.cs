using System.Runtime.CompilerServices;

namespace UniversalEntities
{
    public ref struct FilterEnumerator
    {
        readonly Filter m_filter;
        Entity m_currentValue;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FilterEnumerator(Filter filter)
        {
            m_currentValue = null;
            m_filter = filter;
            filter.Begin();
        }
        
        public Entity Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => m_currentValue;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext()
        {
            return m_filter.TryIterate(out m_currentValue);
        }
    };
}
