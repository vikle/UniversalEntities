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
        Pipeline m_pipeline;
        readonly FragmentStack m_fragmentStack = new FragmentStack();

        internal int Id;
        internal BitMask Mask;

        public bool IsAlive
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]get; 
            [MethodImpl(MethodImplOptions.AggressiveInlining)]private set;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void Init(Pipeline pipeline, int id)
        {
            Id = id;
            IsAlive = true;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void Dispose()
        {
            Mask = default;
            IsAlive = false;
            m_pipeline = null;
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
            
            m_pipeline.OnEntityFragmentAdded(this);

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
            
            m_pipeline.OnEntityFragmentAdded(this);
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
            
            m_pipeline.OnEntityFragmentRemoved(this);
        }
    };
}
