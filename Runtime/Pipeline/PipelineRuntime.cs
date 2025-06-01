using System.Runtime.CompilerServices;

namespace UniversalEntities
{
    partial class Pipeline
    {
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
    };
}
