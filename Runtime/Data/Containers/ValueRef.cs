using System.Runtime.CompilerServices;

namespace UniversalEntities
{
    public sealed unsafe class ValueRef<T> : IResettableComponent where T : unmanaged
    {
        T* m_pointer;
        
        public bool IsValid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => (m_pointer != null);
        }
        
        public ref readonly T ValueRO
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref *m_pointer;
        }
        
        public ref T ValueRW
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref *m_pointer;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Assign(ref T data)
        {
            fixed (T* ptr = &data)
            {
                m_pointer = ptr;
            }
        }
        
        void IResettableComponent.OnReset()
        {
            m_pointer = null;
        }
    };
}
