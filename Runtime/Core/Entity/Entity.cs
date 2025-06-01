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
    public sealed class Entity
    {
        readonly Pipeline m_pipeline;
        readonly FragmentStack m_fragmentStack;

        internal BitMask Mask;
        internal int Index;
        
        public bool IsAlive
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]get; 
            [MethodImpl(MethodImplOptions.AggressiveInlining)]private set;
        }

        public Entity()
        {
            m_pipeline = Pipeline.Instance;
            m_fragmentStack = new FragmentStack();
            Index = -1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void Init(int position)
        {
            IsAlive = true;
            Index = position;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void Dispose()
        {
            IsAlive = false;
            Mask = default;
            Index = -1;
            m_fragmentStack.Clear();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Destroy()
        {
            m_pipeline.DestroyEntity(this);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HasComponent<T>() where T : class, IFragment
        {
            return m_fragmentStack.Has<T>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Trigger<T>() where T : class, IEvent, new()
        {
            return AddComponent<T>();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Then<T>() where T : class, IPromise, new()
        {
            var promise = AddComponent<T>();
            promise.Target = this;
            return promise;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Mark<T>(EPromiseState newState) where T : class, IPromise
        {
            var promise = m_fragmentStack.Get<T>();
            promise.State = newState;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T AddComponent<T>() where T : class, IFragment, new()
        {
            if (!m_fragmentStack.Add<T>(out var instance))
            {
                return instance;
            }
            
            int type_index = FragmentTypeIndex<T>.Index;
            Mask.Set(type_index);

            return instance;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddComponent<T>(T instance) where T : class, IUnmanagedComponent
        {
            if (!m_fragmentStack.Add(instance))
            {
                return;
            }
            
            int type_index = FragmentTypeIndex<T>.Index;
            Mask.Set(type_index);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetComponent<T>() where T : class, IFragment
        {
            return m_fragmentStack.Get<T>();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveComponent<T>() where T : class, IFragment
        {
            if (!m_fragmentStack.Remove<T>())
            {
                return;
            }
            
            int type_index = FragmentTypeIndex<T>.Index;
            Mask.Unset(type_index);
        }
    };
}
