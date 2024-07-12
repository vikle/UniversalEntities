using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace UniversalEntities
{
    public sealed class Context : IContext, IContextBinding, IContextRuntime
    {
        readonly List<IEntity> m_entities = new(4);
        readonly List<ISystem> m_allSystems = new(8);
        readonly List<IUpdateSystem> m_updateSystems = new(8);
        readonly List<IEntityInitializeSystem> m_entityInitializeSystems = new(8);
        readonly List<IEntityTerminateSystem> m_entityTerminateSystems = new(8);
        ArrayList m_injectionsCache = new(8);

        public IEntity CreateEntity<T>() where T : class, IEntity, new()
        {
            var entity = EntityFactory.GetInstance<T>();
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
                case IUpdateSystem update_system: 
                    m_updateSystems.Add(update_system);
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
            
            const BindingFlags k_binding_flags = (BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            
            for (int i = 0, i_max = m_allSystems.Count; i < i_max; i++)
            {
                var system = m_allSystems[i];
                var system_fields = system.GetType().GetFields(k_binding_flags);

                for (int j = 0, j_max = system_fields.Length; j < j_max; j++)
                {
                    var field = system_fields[j];
                    var field_type = field.FieldType;
                    
                    for (int k = 0, k_max = m_injectionsCache.Count; k < k_max; k++)
                    {
                        object injection = m_injectionsCache[k];
                        if (field_type.IsInstanceOfType(injection) == false) continue;
                        field.SetValue(system, injection);
                        break;
                    }
                }
            }
            
            m_injectionsCache.Clear();
            m_injectionsCache = null;
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
        public void OnUpdate()
        {
            for (int i = 0, i_max = m_updateSystems.Count; i < i_max; i++)
            {
                m_updateSystems[i].OnUpdate(this);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ContextEnumerator GetEnumerator()
        {
            return new(m_entities);
        }
    };
}
