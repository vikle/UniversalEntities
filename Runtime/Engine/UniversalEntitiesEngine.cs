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
    [DefaultExecutionOrder(-50), DisallowMultipleComponent]
    public sealed class UniversalEntitiesEngine : MonoBehaviour
    {
        Pipeline m_pipeline;
        
        void Awake()
        {
            m_pipeline = PipelineSingleton.Get;
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
    };
}
