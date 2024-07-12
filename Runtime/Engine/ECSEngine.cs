using UnityEngine;

namespace UniversalEntities
{
    [DefaultExecutionOrder(-100), DisallowMultipleComponent]
    public sealed class ECSEngine : MonoBehaviour
    {
        public MonoBehaviour bootstrap;
        
        public static IContext Context { get; private set; }
        static IContextRuntime s_runtime;

        void Awake()
        {
            var context = new Context();
            Context = context;
            s_runtime = context;
            
            if (bootstrap is IECSBootstrap ecs_bootstrap)
            {
                ecs_bootstrap.OnBootstrap(context);
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
