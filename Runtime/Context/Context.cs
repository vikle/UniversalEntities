using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace UniversalEntities
{
    public sealed class Context : IContext, IContextBinding, IContextRuntime
    {
        readonly List<IEntity> m_entities = new(32);
        
        readonly List<ISystem> m_allSystems = new(128);
        readonly List<IFixedUpdateSystem> m_fixedUpdateSystems = new(16);
        readonly List<IUpdateSystem> m_updateSystems = new(32);
        readonly List<ILateUpdateSystem> m_lateUpdateSystems = new(16);
        readonly List<IEntityInitializeSystem> m_entityInitializeSystems = new(8);
        readonly List<IEntityTerminateSystem> m_entityTerminateSystems = new(8);
        
        ArrayList m_injectionsCache = new(16);

        public IEntity CreateEntity<T>() where T : class, IEntity, new()
        {
            var entity = EntityPool.Get<T>();
            AddEntity(entity);
            return entity;
        }

        public void DestroyEntity(IEntity entity)
        {
            RemoveEntity(entity);
            entity.Dispose();
        }
        
        public void AddEntity(IEntity entity)
        {
            if (m_entities.Contains(entity)) return;

            for (int i = 0, i_max = m_entityInitializeSystems.Count; i < i_max; i++)
            {
                m_entityInitializeSystems[i].OnAfterEntityCreated(this, entity);
            }
            
            m_entities.Add(entity);
        }
    
        public void RemoveEntity(IEntity entity)
        {
            int entity_id = m_entities.IndexOf(entity);
            if (entity_id < 0) return;

            m_entities.RemoveAt(entity_id);
            
            for (int i = 0, i_max = m_entityTerminateSystems.Count; i < i_max; i++)
            {
                m_entityTerminateSystems[i].OnBeforeEntityDestroyed(this, entity);
            }
        }

        public IContextBinding BindEvent<T>() where T : class, IEvent
        {
            return BindSystem<EventCollector<T>>();
        }
        
        public IContextBinding BindPromise<T>() where T : class, IPromise
        {
            return BindSystem<PromiseCollector<T>>();
        }
    
        public IContextBinding BindSystem<T>() where T : class, ISystem, new()
        {
            var bin_system = new T();
        
            m_allSystems.Add(bin_system);
        
            switch (bin_system)
            {
                case IFixedUpdateSystem fixed_update_system: 
                    m_fixedUpdateSystems.Add(fixed_update_system);
                    break;
                case IUpdateSystem update_system: 
                    m_updateSystems.Add(update_system);
                    break;
                case ILateUpdateSystem late_update_system: 
                    m_lateUpdateSystems.Add(late_update_system);
                    break;
                case IEntityInitializeSystem enabled_system: 
                    m_entityInitializeSystems.Add(enabled_system);
                    break;
                case IEntityTerminateSystem disabled_system: 
                    m_entityTerminateSystems.Add(disabled_system);
                    break;
            }

            return this;
        }

        public IContextBinding Inject<T>(T injection) where T : class
        {
            if (m_injectionsCache.Contains(injection) == false)
            {
                m_injectionsCache.Add(injection);
            }
            
            return this;
        }

        public void Init()
        {
            InjectDependencies();
        }

        private void InjectDependencies()
        {
            if (m_injectionsCache.Count == 0) return;
            
            for (int i = 0, i_max = m_allSystems.Count; i < i_max; i++)
            {
                var system = m_allSystems[i];
                InjectDependenciesIn(system);
            }
            
            m_injectionsCache.Clear();
            m_injectionsCache = null;
        }

        private void InjectDependenciesIn(object obj)
        {
            const BindingFlags k_binding_attr = (BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            
            var obj_type = obj.GetType();
            var obj_fields = obj_type.GetFields(k_binding_attr);

            for (int i = 0, i_max = obj_fields.Length; i < i_max; i++)
            {
                var field = obj_fields[i];
                var field_type = field.FieldType;
                    
                for (int j = 0, j_max = m_injectionsCache.Count; j < j_max; j++)
                {
                    object injection = m_injectionsCache[j];
                    if (field_type.IsInstanceOfType(injection) == false) continue;
                    field.SetValue(obj, injection);
                    break;
                }
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void OnStart()
        {
            for (int i = 0, i_max = m_allSystems.Count; i < i_max; i++)
            {
                if (m_allSystems[i] is IStartSystem start_system)
                {
                    start_system.OnStart(this);
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void OnFixedUpdate()
        {
            for (int i = 0, i_max = m_fixedUpdateSystems.Count; i < i_max; i++)
            {
                m_fixedUpdateSystems[i].OnFixedUpdate(this);
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void OnUpdate()
        {
            for (int i = 0, i_max = m_updateSystems.Count; i < i_max; i++)
            {
                m_updateSystems[i].OnUpdate(this);
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void OnLateUpdate()
        {
            for (int i = 0, i_max = m_lateUpdateSystems.Count; i < i_max; i++)
            {
                m_lateUpdateSystems[i].OnLateUpdate(this);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ContextEnumerator GetEnumerator()
        {
            return new(m_entities);
        }
    };
}
