using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace UniversalEntities
{
    public struct ContextEnumerator
    {
        readonly IReadOnlyList<IEntity> m_entities;
        readonly int m_count;
        int m_index;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ContextEnumerator(IReadOnlyList<IEntity> entities)
        {
            m_entities = entities;
            m_count = m_entities.Count;
            m_index = -1;
            Current = default;
        }

        public IEntity Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]get; 
            [MethodImpl(MethodImplOptions.AggressiveInlining)]private set;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext()
        {
            int count = m_count;
            ref int index = ref m_index;
            var entities = m_entities;
            
            if (++index >= count) return false;

            Current = entities[index];
            return true;
        }
    };
}
