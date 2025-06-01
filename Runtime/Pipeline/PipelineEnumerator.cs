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
    public struct PipelineEnumerator
    {
        readonly Entity[] m_sparseEntities;
        readonly int m_denseCount;
        int m_index;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public PipelineEnumerator(Entity[] sparseEntities, int denseCount)
        {
            m_sparseEntities = sparseEntities;
            m_denseCount = denseCount;
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
            int count = m_denseCount;
            ref int index = ref m_index;
            
            if (++index >= count) return false;

            Current = m_sparseEntities[index];
            return true;
        }
    };
}
