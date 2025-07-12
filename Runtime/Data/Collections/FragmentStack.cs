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
    internal sealed class FragmentStack
    {
        int[] m_sparse = new int[32];
        IFragment[] m_dense = new IFragment[8];
        int m_denseCapacity = 1;
        int[] m_recycled = new int[8];
        int m_recycledCount;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal T Get<T>() where T : class, IFragment
        {
            int type_index = FragmentTypeIndex<T>.Index;
#if DEBUG
            if (m_sparse.Length <= type_index)
            {
                throw new System.InvalidOperationException($"Fragment type '{typeof(T)}' is not valid or not registered.");
            }
#endif
            ref int pointer = ref m_sparse[type_index];
#if DEBUG
            if (pointer == 0)
            {
                throw new System.InvalidOperationException($"Component of type '{typeof(T)}' not found on entity.");
            }
#endif
            return (T)m_dense[pointer];
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal bool Has<T>() where T : class, IFragment
        {
            int type_index = FragmentTypeIndex<T>.Index;
            return ((m_sparse.Length > type_index) && (m_sparse[type_index] > 0));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal bool Add<T>(T instance) where T : class, IUnmanagedComponent
        {
            int type_index = FragmentTypeIndex<T>.Index;
            
            ArrayTool.EnsureCapacity(ref m_sparse, FragmentIndexCounter.Count);
            
            ref int pointer = ref m_sparse[type_index];
            
            if (pointer > 0) return false;
            
            pointer = RestoreDenseIndex();
            m_dense[pointer] = instance;

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal bool Add<T>(out T instance) where T : class, IFragment, new()
        {
            int type_index = FragmentTypeIndex<T>.Index;
            
            ArrayTool.EnsureCapacity(ref m_sparse, FragmentIndexCounter.Count);

            ref int pointer = ref m_sparse[type_index];

            if (pointer > 0)
            {
                instance = (T)m_dense[pointer];
                return false;
            }

            instance = FragmentPool.Get<T>();
            pointer = RestoreDenseIndex();
            m_dense[pointer] = instance;

            if (instance is IResettableComponent resettable)
            {
                resettable.OnReset();
            }

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int RestoreDenseIndex()
        {
            if (ArrayTool.TryPop(m_recycled, ref m_recycledCount, out int index))
            {
                return index;
            }
            
            index = m_denseCapacity;
            ArrayTool.EnsureCapacity(ref m_dense, m_denseCapacity++);

            return index;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal bool Remove<T>() where T : class, IFragment
        {
            int type_index = FragmentTypeIndex<T>.Index;
            
            if (m_sparse.Length <= type_index) return false;

            ref int pointer = ref m_sparse[type_index];
            
            if (pointer == 0) return false;

            ClearDenseValue(pointer);
            ArrayTool.Push(ref m_recycled, ref m_recycledCount, in pointer);
            pointer = 0;

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void Clear()
        {
            int[] sparse = m_sparse;
            
            for (int i = 0, i_max = sparse.Length; i < i_max; i++)
            {
                ref int pointer = ref sparse[i];
                if (pointer == 0) continue;
                
                ClearDenseValue(pointer);
                ArrayTool.Push(ref m_recycled, ref m_recycledCount, in pointer);
                pointer = 0;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void ClearDenseValue(int index)
        {
            ref var value = ref m_dense[index];
                
            if (!(value is IUnmanagedComponent))
            {
                FragmentPool.Release(value);
            }
                
            value = null;
        }
    };
}
