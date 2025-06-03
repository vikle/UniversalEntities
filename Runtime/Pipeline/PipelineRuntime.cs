using System.Runtime.CompilerServices;

namespace UniversalEntities
{
    partial class Pipeline
    {
        // IAwake
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void RunAwake()
        {
            for (int i = 0, i_max = m_allSystems.Count; i < i_max; i++)
            {
                if (m_allSystems[i] is IAwakeSystem start_system)
                {
                    start_system.OnAwake(this);
                }
            }
        }
        
        
        
        
        // IStart
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void RunStart()
        {
            for (int i = 0, i_max = m_allSystems.Count; i < i_max; i++)
            {
                if (m_allSystems[i] is IStartSystem start_system)
                {
                    start_system.OnStart(this);
                }
            }
        }

        
        
        
        
        // IFixed Update
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void RunFixedUpdate()
        {
            for (int i = 0, i_max = m_fixedUpdateSystems.Length; i < i_max; i++)
            {
                m_fixedUpdateSystems[i].OnFixedUpdate(this);
            }
        }
        
        
        
        // IUpdate
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void RunUpdate()
        {
            for (int i = 0, i_max = m_updateSystems.Length; i < i_max; i++)
            {
                m_updateSystems[i].OnUpdate(this);
            }
        }
        
        
        
        
        
        // ILate Update
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void RunLateUpdate()
        {
            for (int i = 0, i_max = m_lateUpdateSystems.Length; i < i_max; i++)
            {
                m_lateUpdateSystems[i].OnLateUpdate(this);
            }
        }
        
        
        
        
        
        
        
        
        // IEntity Initialize
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void RunAfterEntityCreated(Entity entity)
        {
            for (int i = 0, i_max = m_entityInitializeSystems.Length; i < i_max; i++)
            {
                m_entityInitializeSystems[i].OnAfterEntityCreated(this, entity);
            }
        }
        
        
        
        
        
        
        // IEntity Terminate
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void RunBeforeEntityDestroyed(Entity entity)
        {
            for (int i = 0, i_max = m_entityTerminateSystems.Length; i < i_max; i++)
            {
                m_entityTerminateSystems[i].OnBeforeEntityDestroyed(this, entity);
            }
        }
        
        
        
        
        
        
        
    };
}
