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

        static Context s_context;
        
        public static Context GetContext
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (s_context != null) return s_context;
                s_context = new Context();
                return s_context;
            }
        }
        
        void Awake()
        {
            var context = GetContext;
            
            if (bootstrap != null)
            {
                bootstrap.OnBootstrap(context);
            }
            
            context.Init();
        }

        void Start()
        {
            s_context.OnStart();
        }

        void FixedUpdate()
        {
            TimeData.OnFixedUpdate();
            s_context.OnFixedUpdate();
        }

        void Update()
        {
            TimeData.OnUpdate();
            s_context.OnUpdate();
        }

        void LateUpdate()
        {
            s_context.OnLateUpdate();
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
