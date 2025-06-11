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
            for (int i = 0, i_max = m_allSystems.Count; i < i_max; i++)
            {
                if (m_allSystems[i] is IAwakeSystem system)
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
            for (int i = 0, i_max = m_allSystems.Count; i < i_max; i++)
            {
                if (m_allSystems[i] is IStartSystem system)
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
            
#if DEBUG && !UNIVERSAL_ENTITIES_RELEASE
            string[] systems_names = m_fixedUpdateSystemsNames;
#endif
            for (int i = 0, i_max = systems.Length; i < i_max; i++)
            {
#if DEBUG && !UNIVERSAL_ENTITIES_RELEASE
                Profiler.BeginSample(systems_names[i]);
            
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
            
#if DEBUG && !UNIVERSAL_ENTITIES_RELEASE
            string[] systems_names = m_updateSystemsNames;
#endif
            for (int i = 0, i_max = systems.Length; i < i_max; i++)
            {
#if DEBUG && !UNIVERSAL_ENTITIES_RELEASE
                Profiler.BeginSample(systems_names[i]);
            
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
            
#if DEBUG && !UNIVERSAL_ENTITIES_RELEASE
            string[] systems_names = m_lateUpdateSystemsNames;
#endif
            for (int i = 0, i_max = systems.Length; i < i_max; i++)
            {
#if DEBUG && !UNIVERSAL_ENTITIES_RELEASE
                Profiler.BeginSample(systems_names[i]);
            
                try { systems[i].OnLateUpdate(this); }
                catch (Exception e) { UniversalEntitiesLogger.LogException(e); }
                
                Profiler.EndSample();
#else
                systems[i].OnLateUpdate(this);
#endif
            }
        }
        
        // IEntity Initialize
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void RunAfterEntityCreated(Entity entity)
        {
            var systems = m_entityInitializeSystems;
            
#if DEBUG && !UNIVERSAL_ENTITIES_RELEASE
            string[] systems_names = m_entityInitializeSystemsNames;
#endif
            for (int i = 0, i_max = systems.Length; i < i_max; i++)
            {
#if DEBUG && !UNIVERSAL_ENTITIES_RELEASE
                Profiler.BeginSample(systems_names[i]);
            
                try { systems[i].OnAfterEntityCreated(this, entity); }
                catch (Exception e) { UniversalEntitiesLogger.LogException(e); }
                
                Profiler.EndSample();
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
            
#if DEBUG && !UNIVERSAL_ENTITIES_RELEASE
            string[] systems_names = m_entityTerminateSystemsNames;
#endif
            for (int i = 0, i_max = systems.Length; i < i_max; i++)
            {
#if DEBUG && !UNIVERSAL_ENTITIES_RELEASE
                Profiler.BeginSample(systems_names[i]);
            
                try { systems[i].OnBeforeEntityDestroyed(this, entity); }
                catch (Exception e) { UniversalEntitiesLogger.LogException(e); }
                
                Profiler.EndSample();
#else
                systems[i].OnBeforeEntityDestroyed(this, entity);
#endif
            }
        }
    };
}
