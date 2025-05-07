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
    public sealed class Entity : IEntity
    {
        readonly Dictionary<Type, IFragment> m_fragmentsMap = new Dictionary<Type, IFragment>(8);
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool HasComponent<T>() where T : class, IFragment
        {
            var type = typeof(T);
            return m_fragmentsMap.ContainsKey(type);
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
            if (TryGetComponent(out T promise))
            {
                promise.State = newState;
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T AddComponent<T>() where T : class, IFragment, new()
        {
            var type = typeof(T);
            
            if (m_fragmentsMap.TryGetValue(type, out var raw) && raw is T instance)
            {
                return instance;
            }
            
            instance = FragmentPool.Get<T>();
            m_fragmentsMap[type] = instance;
            
            return instance;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddComponent<T>(T instance) where T : class, IFragment
        {
            if (instance == null) return;
            
            var type = instance.GetType();
            m_fragmentsMap[type] = instance;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetComponent<T>(out T fragment) where T : class, IFragment
        {
            var type = typeof(T);
            
            if (m_fragmentsMap.TryGetValue(type, out var raw) && raw is T instance)
            {
                fragment = instance;
                return true;
            }

            fragment = null;
            return false;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveComponent<T>() where T : class, IFragment
        {
            var type = typeof(T);
            
            if (m_fragmentsMap.TryGetValue(type, out var instance))
            {
                RemoveComponent(instance);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveComponent<T>(T instance) where T : class, IFragment
        {
            if (instance == null) return;
            
            RemoveFromMap(instance);
            FragmentPool.Release(instance);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveComponent(EntityActorComponent instance)
        {
            if (instance != null)
            {
                RemoveFromMap(instance);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void RemoveFromMap(IFragment instance)
        {
            var type = instance.GetType();
            m_fragmentsMap.Remove(type);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose()
        {
            EntityPool.Release(this);
            
            foreach (var instance in m_fragmentsMap.Values)
            {
                FragmentPool.Release(instance);
            }
            
            m_fragmentsMap.Clear();
        }
    };
}
