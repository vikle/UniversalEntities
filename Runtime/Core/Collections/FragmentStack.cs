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
        internal bool Add<T>(T instance) where T : class, IFragment
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
        internal bool Remove<T>(T instance) where T : class, IFragment
        {
            int type_index = FragmentTypeIndex<T>.Index;

            int[] sparse = m_sparse;
            
            if (sparse.Length <= type_index) return false;

            ref int pointer = ref sparse[type_index];

            m_dense[pointer] = null;
            
            pointer = 0;
            m_denseCount--;
            
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal bool Remove<T>() where T : class, IFragment
        {
            int type_index = FragmentTypeIndex<T>.Index;

            int[] sparse = m_sparse;
            
            if (sparse.Length <= type_index) return false;

            ref int pointer = ref sparse[type_index];
            
            if (pointer == 0) return false;

            m_denseCount--;
            
            ref var value = ref m_dense[pointer];
            FragmentPool.Release(value);
            value = null;

            pointer = 0;

            return true;
        }
    };
}
