using System.Reflection;
using System.Collections;
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
    public sealed class Context
    {
        readonly List<Entity> m_entities = new List<Entity>(32);
        readonly List<ISystem> m_allSystems = new List<ISystem>(128);
        
        IFixedUpdateSystem[] m_fixedUpdateSystems;
        IUpdateSystem[] m_updateSystems;
        ILateUpdateSystem[] m_lateUpdateSystems;
        IEntityInitializeSystem[] m_entityInitializeSystems;
        IEntityTerminateSystem[] m_entityTerminateSystems;
        
        ArrayList m_injectionsCache = new ArrayList(16);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Entity CreateEntity()
        {
            var entity = EntityPool.Get();
            AddEntity(entity);
            return entity;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DestroyEntity(Entity entity)
        {
            RemoveEntity(entity);
            entity.Dispose();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void AddEntity(Entity entity)
        {
            if (m_entities.Contains(entity)) return;

            for (int i = 0, i_max = m_entityInitializeSystems.Length; i < i_max; i++)
            {
                m_entityInitializeSystems[i].OnAfterEntityCreated(this, entity);
            }
            
            m_entities.Add(entity);
        }
    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveEntity(Entity entity)
        {
            int entity_id = m_entities.IndexOf(entity);
            if (entity_id < 0) return;

            m_entities.RemoveAt(entity_id);
            
            for (int i = 0, i_max = m_entityTerminateSystems.Length; i < i_max; i++)
            {
                m_entityTerminateSystems[i].OnBeforeEntityDestroyed(this, entity);
            }
        }

        public Context BindEvent<T>() where T : class, IEvent
        {
            return BindSystem<EventCollector<T>>();
        }
        
        public Context BindPromise<T>() where T : class, IPromise
        {
            return BindSystem<PromiseCollector<T>>();
        }
        
        public Context BindSystem<T>() where T : class, ISystem, new()
        {
            m_allSystems.Add(new T());
            return this;
        }

        public Context Inject<T>(T injection) where T : class
        {
            if (m_injectionsCache.Contains(injection) == false)
            {
                m_injectionsCache.Add(injection);
            }
            
            return this;
        }

        internal void Init()
        {
            CastSystems();
            InjectDependencies();
        }

        private void CastSystems()
        {
            var all_systems = m_allSystems.ToArray();
            
            ArrayTool.WhereCast(all_systems, out m_updateSystems);
            ArrayTool.WhereCast(all_systems, out m_fixedUpdateSystems);
            ArrayTool.WhereCast(all_systems, out m_lateUpdateSystems);
            ArrayTool.WhereCast(all_systems, out m_entityInitializeSystems);
            ArrayTool.WhereCast(all_systems, out m_entityTerminateSystems);
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
                    if (!field_type.IsInstanceOfType(injection)) continue;
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
            for (int i = 0, i_max = m_fixedUpdateSystems.Length; i < i_max; i++)
            {
                m_fixedUpdateSystems[i].OnFixedUpdate(this);
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void OnUpdate()
        {
            for (int i = 0, i_max = m_updateSystems.Length; i < i_max; i++)
            {
                m_updateSystems[i].OnUpdate(this);
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void OnLateUpdate()
        {
            for (int i = 0, i_max = m_lateUpdateSystems.Length; i < i_max; i++)
            {
                m_lateUpdateSystems[i].OnLateUpdate(this);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ContextEnumerator GetEnumerator()
        {
            return new ContextEnumerator(m_entities);
        }
    };
}
