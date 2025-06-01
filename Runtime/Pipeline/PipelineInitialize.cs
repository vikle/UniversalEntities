using System.Reflection;

namespace UniversalEntities
{
    partial class Pipeline
    {
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
            var bind_flags = (BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            
            var obj_type = obj.GetType();
            var obj_fields = obj_type.GetFields(bind_flags);

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
    };
}
