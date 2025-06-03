using System;
using System.Runtime.CompilerServices;
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
        
        static UniversalEntitiesEngine s_validInstance;
        
        void Awake()
        {
            if (s_validInstance != null && s_validInstance != this)
            {
                Destroy(gameObject);
                return;
            }
            
            s_validInstance = this;
            
            DontDestroyOnLoad(gameObject);
            
            m_pipeline = Pipeline.Instance;
            
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
        }
        
#if UNITY_EDITOR
        [UnityEditor.MenuItem("Tools/Universal Entities/Create/Engine", true)]
        private static bool CanCreateEngine()
        {
            return (FindObjectOfType<UniversalEntitiesEngine>() == null);
        }

        [UnityEditor.MenuItem("Tools/Universal Entities/Create/Engine")]
        private static void CreateEngine()
        {
            var obj_type = typeof(UniversalEntitiesEngine);
            CreateGameObject(obj_type);
        }

        [UnityEditor.MenuItem("Tools/Universal Entities/Create/Bootstrap", true)]
        private static bool CanCreateBootstrap()
        {
            return (FindObjectOfType<UniversalEntitiesBootstrap>() == null);
        }

        [UnityEditor.MenuItem("Tools/Universal Entities/Create/Bootstrap")]
        private static void CreateBootstrap()
        {
            var obj_type = typeof(UniversalEntitiesBootstrap);
            CreateGameObject(obj_type);
        }

        private static void CreateGameObject(Type objType)
        {
            var main_obj = new GameObject(objType.Name, objType)
            {
                isStatic = true
            };
            
            UnityEditor.Selection.activeObject = main_obj;
        }
#endif
    };
}
