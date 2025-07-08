using UnityEngine;

#if ENABLE_IL2CPP
using Unity.IL2CPP.CompilerServices;
#endif

namespace UniversalEntities
{
#if ENABLE_IL2CPP
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
#endif
    [DefaultExecutionOrder(-100), DisallowMultipleComponent]
    public sealed class UniversalEntitiesStarter : MonoBehaviour
    {
        [SerializeField]bool m_keepAlive = true;
        [SerializeField]UniversalEntitiesBootstrap m_bootstrap;
        
        Pipeline m_pipeline;
        
        void Awake()
        {
            if (m_keepAlive)
            {
                DontDestroyOnLoad(gameObject);
            }
            
            PipelineSingleton.Initialize();
            m_pipeline = PipelineSingleton.Get;
            
            if (m_bootstrap != null)
            {
                m_bootstrap.OnBootstrap(m_pipeline);
            }
            
            m_pipeline.Init();

            gameObject.AddComponent<UniversalEntitiesEngine>();
            gameObject.AddComponent<UniversalEntitiesCollector>();
        }
        
#if UNITY_EDITOR
        [UnityEditor.MenuItem("Tools/Universal Entities/Create Starter", true)]
        private static bool CanCreateEngine()
        {
            return (FindObjectOfType<UniversalEntitiesStarter>() == null);
        }

        [UnityEditor.MenuItem("Tools/Universal Entities/Create Starter")]
        private static void CreateEngine()
        {
            var obj_type = typeof(UniversalEntitiesStarter);
            
            var root_obj = new GameObject(obj_type.Name, obj_type)
            {
                isStatic = true
            };
            
            UnityEditor.Selection.activeObject = root_obj;
        }
#endif
    };
}
