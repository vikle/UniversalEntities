using System;
using System.Collections.Generic;

namespace UniversalEntities
{
    public sealed class Entity : IEntity
    {
        readonly Dictionary<Type, IFragment> m_fragmentsMap = new(8);
        
        public bool Has<T>() where T : class, IFragment
        {
            var type = typeof(T);
            return m_fragmentsMap.ContainsKey(type);
        }

        public T Trigger<T>() where T : class, IEvent, new()
        {
            return Add<T>();
        }
        
        public T Then<T>() where T : class, IPromise, new()
        {
            var promise = Add<T>();
            promise.Target = this;
            return promise;
        }

        public void Mark<T>(EPromiseState newState) where T : class, IPromise
        {
            if (TryGet(out T promise))
            {
                promise.State = newState;
            }
        }
        
        public T Add<T>() where T : class, IFragment, new()
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

        public void Add<T>(T instance) where T : class, IFragment
        {
            if (instance == null) return;
            
            var type = instance.GetType();
            m_fragmentsMap[type] = instance;
        }

        public bool TryGet<T>(out T fragment) where T : class, IFragment
        {
            var type = typeof(T);
            
            if (m_fragmentsMap.TryGetValue(type, out var raw) && raw is T instance)
            {
                fragment = instance;
                return true;
            }

            fragment = default;
            return false;
        }
        
        public void Remove<T>() where T : class, IFragment
        {
            var type = typeof(T);
            
            if (m_fragmentsMap.TryGetValue(type, out var instance))
            {
                Remove(instance);
            }
        }

        public void Remove<T>(T instance) where T : class, IFragment
        {
            if (instance == null) return;
            
            RemoveFromMap(instance);
            FragmentPool.Release(instance);
        }
        
        public void Remove(EntityActorComponent instance)
        {
            if (instance != null)
            {
                RemoveFromMap(instance);
            }
        }

        private void RemoveFromMap(IFragment instance)
        {
            var type = instance.GetType();
            m_fragmentsMap.Remove(type);
        }

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
