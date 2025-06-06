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
    public ref struct SparseEntitiesEnumerator
    {
        readonly Entity[] m_entities; // sparse array
        readonly int[] m_dense;       // dense indices
        readonly int m_count;
        int m_iterator;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SparseEntitiesEnumerator(Entity[] entities, int[] dense, int count)
        {
            m_entities = entities;
            m_dense = dense;
            m_count = count;
            m_iterator = -1;
            Current = null;
        }
        
        public Entity Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]get; 
            [MethodImpl(MethodImplOptions.AggressiveInlining)]private set;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext()
        {
            ref int iterator = ref m_iterator;

            if (++iterator >= m_count)
            {
                return false;
            }

            Current = m_entities[m_dense[iterator]];
            return true;
        }
    };
}
