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
    public sealed class Filter
    {
        readonly BitMask m_mask;
        readonly SparseSet m_sparseSet;
        readonly Pipeline m_pipeline;
        
        bool m_locked;
        internal int m_waitersCount;
        internal (bool, int)[] m_waiters;
        
        public int EntityCount
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => m_sparseSet.m_count;
        }
        
        public bool IsEmpty
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => (m_sparseSet.m_count == 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal Filter(Pipeline pipeline, in BitMask mask)
        {
            m_pipeline = pipeline;
            m_sparseSet = new SparseSet();
            m_waiters = new (bool, int)[8];
            m_mask = mask;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal bool IsCompatibleWith(BitMask entityMask)
        {
            return entityMask.Has(in m_mask);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void Lock()
        {
#if DEBUG && !UNIVERSAL_ENTITIES_RELEASE
            if (m_locked)
            {
                throw new System.Exception("Try iterate locked filter.");
            }
#endif
            m_locked = true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void Unlock()
        {
#if DEBUG && !UNIVERSAL_ENTITIES_RELEASE
            if (!m_locked)
            {
                throw new System.Exception("Try end unlocked filter.");
            }
#endif
            m_locked = false;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void EnsureCapacity(int capacity)
        {
            m_sparseSet.EnsureSparseCapacity(capacity);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void AddEntity(int entityId)
        {
            if (m_locked)
            {
                ArrayTool.Push(ref m_waiters, ref m_waitersCount, (true, entityId));
            }
            else
            {
                m_sparseSet.Add(entityId);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void RemoveEntity(int entityId)
        {
            if (m_locked)
            {
                ArrayTool.Push(ref m_waiters, ref m_waitersCount, (false, entityId));
            }
            else
            {
                m_sparseSet.Remove(entityId);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Contains(int entityId)
        {
            return m_sparseSet.Contains(entityId);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FilterEnumerator GetEnumerator()
        {
            return new FilterEnumerator(this, m_sparseSet, m_pipeline.m_sparseEntities);
        }
    };
}
