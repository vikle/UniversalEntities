using System.Runtime.CompilerServices;

namespace UniversalEntities
{
    internal sealed class FragmentStack
    {
        int[] m_sparse = new int[16];
        IFragment[] m_dense = new IFragment[2];
        int m_denseCount = 1;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal T Get<T>() where T : class, IFragment
        {
            int type_index = FragmentTypeIndex<T>.Index;

            if (m_sparse.Length <= type_index)
            {
                throw new System.InvalidOperationException($"Fragment type '{typeof(T)}' is not valid or not registered.");
            }
            
            ref int pointer = ref m_sparse[type_index];

            if (pointer == 0)
            {
                throw new System.InvalidOperationException($"Component of type '{typeof(T)}' not found on entity.");
            }
            
            return (T)m_dense[pointer];
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal bool Has<T>() where T : class, IFragment
        {
            int type_index = FragmentTypeIndex<T>.Index;
            if (m_sparse.Length <= type_index) return false;
            return (m_sparse[type_index] > 0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal bool Add<T>(T instance) where T : class, IUnmanagedComponent
        {
            ArrayTool.EnsureCapacity(ref m_sparse, FragmentIndexCounter.count);

            int type_index = FragmentTypeIndex<T>.Index;
            ref int pointer = ref m_sparse[type_index];
            
            if (pointer > 0) return false;
            
            pointer = m_denseCount;
            ArrayTool.Push(ref m_dense, ref m_denseCount, instance);
            
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal bool Add<T>(out T instance) where T : class, IFragment, new()
        {
            ArrayTool.EnsureCapacity(ref m_sparse, FragmentIndexCounter.count);

            int type_index = FragmentTypeIndex<T>.Index;
            ref int pointer = ref m_sparse[type_index];

            if (pointer > 0)
            {
                instance = (T)m_dense[pointer];
                return false;
            }

            pointer = m_denseCount;
            instance = FragmentPool.Get<T>();
            ArrayTool.Push(ref m_dense, ref m_denseCount, instance);
            
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal bool Remove<T>() where T : class, IFragment
        {
            int type_index = FragmentTypeIndex<T>.Index;
            
            if (m_sparse.Length <= type_index) return false;

            ref int pointer = ref m_sparse[type_index];
            
            if (pointer == 0) return false;

            ClearDense(pointer);
            
            m_denseCount--;
            pointer = 0;

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void Clear()
        {
            for (int i = 0, i_max = m_sparse.Length; i < i_max; i++)
            {
                m_sparse[i] = 0;
            }
            
            for (int i = 0; i < m_denseCount; i++)
            {
                ClearDense(i);
            }
            
            m_denseCount = 0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ClearDense(int index)
        {
            ref var value = ref m_dense[index];
                
            if (value is not IUnmanagedComponent)
            {
                FragmentPool.Release(value);
            }
                
            value = null;
        }
    };
}
