using System;
using System.Runtime.CompilerServices;

#if DEBUG && !UNIVERSAL_ENTITIES_RELEASE
using UnityEngine.Profiling;
#endif

namespace UniversalEntities
{
#if DEBUG && !UNIVERSAL_ENTITIES_RELEASE
    using Logging;
#endif
    
    partial class Pipeline
    {
        // IAwake
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void RunAwake()
        {
            var systems = m_allSystems;
            
            for (int i = 0, i_max = systems.Count; i < i_max; i++)
            {
                if (systems[i] is IAwakeSystem system)
                {
#if DEBUG && !UNIVERSAL_ENTITIES_RELEASE
                    try { system.OnAwake(this); }
                    catch (Exception e) { UniversalEntitiesLogger.LogException(e); }
#else
                    system.OnAwake(this);
#endif
                }
            }
        }
        
        // IStart
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void RunStart()
        {
            var systems = m_allSystems;
            
            for (int i = 0, i_max = systems.Count; i < i_max; i++)
            {
                if (systems[i] is IStartSystem system)
                {
#if DEBUG && !UNIVERSAL_ENTITIES_RELEASE
                    try { system.OnStart(this); }
                    catch (Exception e) { UniversalEntitiesLogger.LogException(e); }
#else
                    system.OnStart(this);
#endif
                }
            }
        }

        // IFixed Update
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void RunFixedUpdate()
        {
            var systems = m_fixedUpdateSystems;
            
            for (int i = 0, i_max = systems.Length; i < i_max; i++)
            {
#if DEBUG && !UNIVERSAL_ENTITIES_RELEASE
                Profiler.BeginSample(m_fixedUpdateSystemsNames[i]);
            
                try { systems[i].OnFixedUpdate(this); }
                catch (Exception e) { UniversalEntitiesLogger.LogException(e); }
                
                Profiler.EndSample();
#else
                systems[i].OnFixedUpdate(this);
#endif
            }
        }
        
        // IUpdate
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void RunUpdate()
        {
            var systems = m_updateSystems;
            
            for (int i = 0, i_max = systems.Length; i < i_max; i++)
            {
#if DEBUG && !UNIVERSAL_ENTITIES_RELEASE
                Profiler.BeginSample(m_updateSystemsNames[i]);
            
                try { systems[i].OnUpdate(this); }
                catch (Exception e) { UniversalEntitiesLogger.LogException(e); }
                
                Profiler.EndSample();
#else
                systems[i].OnUpdate(this);
#endif
            }
        }
        
        // ILate Update
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void RunLateUpdate()
        {
            var systems = m_lateUpdateSystems;
            
            for (int i = 0, i_max = systems.Length; i < i_max; i++)
            {
#if DEBUG && !UNIVERSAL_ENTITIES_RELEASE
                Profiler.BeginSample(m_lateUpdateSystemsNames[i]);
            
                try { systems[i].OnLateUpdate(this); }
                catch (Exception e) { UniversalEntitiesLogger.LogException(e); }
                
                Profiler.EndSample();
#else
                systems[i].OnLateUpdate(this);
#endif
            }
        }
        
        // ICollect
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void RunCollect()
        {
            var systems = m_collectSystems;
            
            for (int i = 0, i_max = systems.Length; i < i_max; i++)
            {
                systems[i].OnCollect(this);
            }
        }
        
        // IEntity Initialize
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void RunAfterEntityCreated(Entity entity)
        {
            var systems = m_entityInitializeSystems;
            
            for (int i = 0, i_max = systems.Length; i < i_max; i++)
            {
#if DEBUG && !UNIVERSAL_ENTITIES_RELEASE
                try { systems[i].OnAfterEntityCreated(this, entity); }
                catch (Exception e) { UniversalEntitiesLogger.LogException(e); }
#else
                systems[i].OnAfterEntityCreated(this, entity);
#endif
            }
        }
        
        // IEntity Terminate
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void RunBeforeEntityDestroyed(Entity entity)
        {
            var systems = m_entityTerminateSystems;
            
            for (int i = 0, i_max = systems.Length; i < i_max; i++)
            {
#if DEBUG && !UNIVERSAL_ENTITIES_RELEASE
                try { systems[i].OnBeforeEntityDestroyed(this, entity); }
                catch (Exception e) { UniversalEntitiesLogger.LogException(e); }
#else
                systems[i].OnBeforeEntityDestroyed(this, entity);
#endif
            }
        }
    };
}
