using System.Runtime.CompilerServices;

#if ENABLE_IL2CPP
using Unity.IL2CPP.CompilerServices;
#endif

namespace UniversalEntities
{
#if ENABLE_IL2CPP
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
#endif
    public ref struct FilterEnumerator
    {
        readonly Filter m_filter;
        readonly SparseSet m_sparseSet;
        readonly int[] m_filterDense;
        readonly int m_filterDenseCount;
        readonly Entity[] m_sparseEntities;

        int m_iterator;
        Entity m_currentValue;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal FilterEnumerator(Filter filter, SparseSet sparseSet, Entity[] sparseEntities)
        {
            m_filter = filter;
            m_sparseSet = sparseSet;
            m_filterDense = sparseSet.m_dense;
            m_filterDenseCount = sparseSet.m_count;
            m_sparseEntities = sparseEntities;

            m_iterator = -1;
            m_currentValue = null;

            filter.Lock();
        }
        
        public Entity Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => m_currentValue;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext()
        {
            if (++m_iterator >= m_filterDenseCount)
            {
                m_currentValue = null;
                return false;
            }
        
            m_currentValue = m_sparseEntities[m_filterDense[m_iterator]];
            return true;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose()
        {
            var f = m_filter;
            
            f.Unlock();
            
            ref int waiters_count = ref f.m_waitersCount;
            
            if (waiters_count == 0) return;
            
            var waiters = f.m_waiters;
            var sparse_set = m_sparseSet;
            
            for (int i = 0, i_max = waiters_count; i < i_max; i++)
            {
                (bool is_add, int entity) = waiters[i];
                
                if (is_add) sparse_set.Add(entity);
                else sparse_set.Remove(entity);
            }

            waiters_count = 0;
        }
    };
}
