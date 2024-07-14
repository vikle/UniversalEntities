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
    };
}
