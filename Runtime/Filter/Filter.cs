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
        
        Entity[] m_sparse;
        Entity[] m_dense;
        int m_denseCount;
        
        int m_iterator;
        bool m_locked;
        int m_waitersCount;
        (bool, Entity)[] m_waiters;
        
        public int EntityCount
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => m_denseCount;
        }
        
        public bool IsEmpty
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => (m_denseCount == 0);
        }
        
        public Entity FirstEntity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => m_dense[0];
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal Filter(int capacity)
        {
            m_dense = new Entity[16];
            m_sparse = new Entity[capacity];
            m_waiters = new (bool, Entity)[2];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal Filter(int capacity, in BitMask mask) : this(capacity)
        {
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

            if (++iterator >= m_denseCount)
            {
                // End();
                entity = null;
                return false;
            }

            entity = m_dense[iterator];
            return true;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void EnsureCapacity(int capacity)
        {
            ArrayTool.EnsureCapacity(ref m_sparse, capacity);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void AddEntity(Entity entity)
        {
            if (m_locked)
            {
                ArrayTool.Push(ref m_waiters, ref m_waitersCount, (true, entity));
                return;
            }

            AddEntityInternal(entity);
        }
        
        // ReSharper disable Unity.PerformanceAnalysis
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void AddEntityInternal(Entity entity)
        {
            ref var pointer = ref m_sparse[entity.Index];

            if (pointer != null) return;

            ArrayTool.Push(ref m_dense, ref m_denseCount, in entity);
            pointer = entity;
        }
        
        
        // ReSharper disable Unity.PerformanceAnalysis
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void RemoveEntityInternal000(Entity entity)
        {
            var sparse = m_sparse;
            ref var pointer = ref sparse[entity.Index];

            if (pointer == null) return;
            
            int index = (pointer.Index - 1);
            pointer = null;
            
            int last_index = --m_denseCount;

            if (index >= last_index) return;

            var dense = m_dense;
            ref var current = ref dense[index];
            current = dense[last_index];
            sparse[current.Index] = current;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void RemoveEntityInternal(Entity entity)
        {
            var sparse = m_sparse;
            int sparse_pos = entity.Index;

            if (sparse[sparse_pos] != entity) return;

            sparse[sparse_pos] = null;

            var dense = m_dense;
            
            int index = -1;
            
            for (int i = 0, i_max = m_denseCount; i < i_max; i++)
            {
                if (dense[i] != entity) continue;
                index = i;
                break;
            }

            if (index == -1) return;

            int last_index = --m_denseCount;

            if (index == last_index) return;
            
            var last = dense[last_index];
            dense[index] = last;

            sparse[last.Index] = last;
        }

        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Contains(Entity entity)
        {
            return (m_sparse[entity.Index] != null);
        }
        
        
        
        
        
        
        
        
        
    };
}
