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
        
        int m_iterator;
        bool m_locked;
        int m_waitersCount;
        (bool, int)[] m_waiters;
        
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
        public void Begin()
        {
#if DEBUG
            if (m_locked)
            {
                throw new System.Exception("Try iterate locked filter.");
            }
#endif
            m_locked = true;
            m_iterator = -1;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryIterate(out Entity entity)
        {
            ref int iterator = ref m_iterator;
        
            if (++iterator >= m_sparseSet.m_count)
            {
                End();
                entity = null;
                return false;
            }
        
            entity = m_pipeline.m_sparseEntities[m_sparseSet.m_dense[iterator]];
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void End()
        {
#if DEBUG
            if (!m_locked)
            {
                throw new System.Exception("Try end unlocked filter.");
            }
#endif
            m_locked = false;

            if (m_waitersCount == 0) return;
            
            var waiters = m_waiters;
            var sparse_set = m_sparseSet;
            
            for (int i = 0, i_max = m_waitersCount; i < i_max; i++)
            {
                (bool is_add, int entity) = waiters[i];
                
                if (is_add) sparse_set.Add(entity);
                else sparse_set.Remove(entity);
            }

            m_waitersCount = 0;
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
                return;
            }

            m_sparseSet.Add(entityId);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void RemoveEntity(int entityId)
        {
            if (m_locked)
            {
                ArrayTool.Push(ref m_waiters, ref m_waitersCount, (false, entityId));
                return;
            }

            m_sparseSet.Remove(entityId);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Contains(int entityId)
        {
            return m_sparseSet.Contains(entityId);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FilterEnumerator GetEnumerator()
        {
            return new FilterEnumerator(this);
        }
    };
}
