#if DEBUG && !UNIVERSAL_ENTITIES_RELEASE
using System.Linq;
#endif

namespace UniversalEntities
{
    partial class Pipeline
    {
        internal void Init()
        {
            CastSystems();
        }

        private void CastSystems()
        {
            var all_systems = m_allSystems.ToArray();
            
            ArrayTool.WhereCast(all_systems, out m_updateSystems);
            ArrayTool.WhereCast(all_systems, out m_fixedUpdateSystems);
            ArrayTool.WhereCast(all_systems, out m_lateUpdateSystems);
            ArrayTool.WhereCast(all_systems, out m_collectSystems);
            ArrayTool.WhereCast(all_systems, out m_entityInitializeSystems);
            ArrayTool.WhereCast(all_systems, out m_entityTerminateSystems);
            
#if DEBUG && !UNIVERSAL_ENTITIES_RELEASE
            m_fixedUpdateSystemsNames = m_fixedUpdateSystems.Select(s => s.GetType().Name).ToArray();
            m_updateSystemsNames = m_updateSystems.Select(s => s.GetType().Name).ToArray();
            m_lateUpdateSystemsNames = m_lateUpdateSystems.Select(s => s.GetType().Name).ToArray();
#endif
        }
    };
}
