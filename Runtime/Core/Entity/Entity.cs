using System;
using System.Collections.Generic;
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
        readonly Context m_context = UniversalEntitiesEngine.GetContext;
        readonly FragmentStack m_fragmentStack = new FragmentStack();

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
            if (m_fragmentStack.Add<T>(out var instance))
            {
                
            }
            
            return instance;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddComponent<T>(T instance) where T : class, IUnmanagedComponent
        {
            if (m_fragmentStack.Add(instance))
            {
                
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetComponent<T>() where T : class, IFragment
        {
            return m_fragmentStack.Get<T>();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveComponent<T>() where T : class, IFragment
        {
            if (m_fragmentStack.Remove<T>())
            {
                
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose()
        {
            EntityPool.Release(this);
            m_fragmentStack.Clear();
        }
    };
}
