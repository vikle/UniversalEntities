using System.Collections.Generic;
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
    public struct ContextEnumerator
    {
        readonly IReadOnlyList<Entity> m_entities;
        readonly int m_count;
        int m_index;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ContextEnumerator(IReadOnlyList<Entity> entities)
        {
            m_entities = entities;
            m_count = m_entities.Count;
            m_index = -1;
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
            int count = m_count;
            ref int index = ref m_index;
            var entities = m_entities;
            
            if (++index >= count) return false;

            Current = entities[index];
            return true;
        }
    };
}
