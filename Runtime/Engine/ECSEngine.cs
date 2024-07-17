using System;
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
    public sealed class ECSEngine : MonoBehaviour
    {
        public ECSBootstrap bootstrap;
        
        public static IContext Context { get; private set; }
        static IContextRuntime s_runtime;

        void Awake()
        {
            var context = new Context();
            Context = context;
            s_runtime = context;
            
            if (bootstrap != null)
            {
                bootstrap.OnBootstrap(context);
            }
            
            context.Init();
        }

        void Start()
        {
            s_runtime.OnStart();
        }

        void FixedUpdate()
        {
            TimeData.OnFixedUpdate();
            s_runtime.OnFixedUpdate();
        }

        void Update()
        {
            TimeData.OnUpdate();
            s_runtime.OnUpdate();
        }

        void LateUpdate()
        {
            s_runtime.OnLateUpdate();
        }
        
#if UNITY_EDITOR
        [UnityEditor.MenuItem("Tools/Universal Entities/Create/Engine", true)]
        private static bool CanCreateEngine()
        {
            return (FindObjectOfType<ECSEngine>() == null);
        }

        [UnityEditor.MenuItem("Tools/Universal Entities/Create/Engine")]
        private static void CreateEngine()
        {
            var obj_type = typeof(ECSEngine);
            CreateGameObject(obj_type);
        }

        [UnityEditor.MenuItem("Tools/Universal Entities/Create/Bootstrap", true)]
        private static bool CanCreateBootstrap()
        {
            return (FindObjectOfType<ECSBootstrap>() == null);
        }

        [UnityEditor.MenuItem("Tools/Universal Entities/Create/Bootstrap")]
        private static void CreateBootstrap()
        {
            var obj_type = typeof(ECSBootstrap);
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
