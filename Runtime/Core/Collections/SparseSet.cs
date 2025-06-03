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
    internal class SparseSet
    {
        protected int[] m_sparse;
        internal int[] m_dense;
        internal int m_count;
        
        internal SparseSet(int capacity = 32)
        {
            m_dense = new int[capacity];
            m_sparse = new int[capacity << 1];
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void EnsureSparseCapacity(int capacity)
        {
            ArrayTool.EnsureCapacity(ref m_sparse, capacity);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void EnsureDenseCapacity(int capacity)
        {
            ArrayTool.EnsureCapacity(ref m_dense, capacity);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void Add(int value)
        {
            ref int pointer = ref m_sparse[value];

            if (pointer > 0) return;
            
            ArrayTool.Push(ref m_dense,ref m_count, value);
            
            pointer = m_count;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void Remove(int value)
        {
            int[] sparse = m_sparse;
            ref int pointer = ref sparse[value];

            if (pointer == 0) return;
            
            int index = (pointer - 1);
            pointer = 0;
            
            int last_index = --m_count;

            if (index == last_index) return;

            int[] dense = m_dense;
            ref int current = ref dense[index];
            current = dense[last_index];
            sparse[current] = (index + 1);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal bool Contains(int value)
        {
            return (m_sparse[value] > 0);
        }
    };

    internal sealed class AllocableSparseSet : SparseSet
    {
        int[] m_recycled;
        int m_recycledCount;
        
        internal AllocableSparseSet(int initialCapacity = 32, int recycledCapacity = 8) : base(initialCapacity)
        {
            m_recycled = new int[recycledCapacity];
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal int Alloc()
        {
            if (!ArrayTool.TryPop(m_recycled, ref m_recycledCount, out int index))
            {
                index = m_count;
                
                EnsureSparseCapacity(index);
                EnsureDenseCapacity(m_count + 1);
            }
            
            m_dense[m_count++] = index;
            m_sparse[index] = m_count;
            
            return index;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void Free(int value)
        {
            ArrayTool.Push(ref m_recycled, ref m_recycledCount, value);
            Remove(value);
        }
    };
}
