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
    public sealed class UniversalEntitiesEngine : MonoBehaviour
    {
        public UniversalEntitiesBootstrap bootstrap;
        
        Pipeline m_pipeline;
        
        void Awake()
        {
            m_pipeline = PipelineSingleton.Get;
            
            if (bootstrap != null)
            {
                bootstrap.OnBootstrap(m_pipeline);
            }
            
            m_pipeline.Init();
            m_pipeline.RunAwake();
        }

        void Start()
        {
            m_pipeline.RunStart();
        }

        void FixedUpdate()
        {
            TimeData.OnFixedUpdate();
            m_pipeline.RunFixedUpdate();
        }

        void Update()
        {
            TimeData.OnUpdate();
            m_pipeline.RunUpdate();
        }

        void LateUpdate()
        {
            m_pipeline.RunLateUpdate();
            m_pipeline.RunCollect();
        }

        void OnDestroy()
        {
            PipelineSingleton.Dispose();
        }

#if UNITY_EDITOR
        [UnityEditor.MenuItem("Tools/Universal Entities/Create Engine", true)]
        private static bool CanCreateEngine()
        {
            return (FindObjectOfType<UniversalEntitiesEngine>() == null);
        }

        [UnityEditor.MenuItem("Tools/Universal Entities/Create Engine")]
        private static void CreateEngine()
        {
            var obj_type = typeof(UniversalEntitiesEngine);
            
            var root_obj = new GameObject(obj_type.Name, obj_type)
            {
                isStatic = true
            };
            
            UnityEditor.Selection.activeObject = root_obj;
        }
#endif
    };
}
