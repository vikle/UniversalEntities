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
    [DefaultExecutionOrder(100), DisallowMultipleComponent]
    public sealed class UniversalEntitiesCollector : MonoBehaviour
    {
        Pipeline m_pipeline;

        void Awake()
        {
            m_pipeline = PipelineSingleton.Get;
        }

        void LateUpdate()
        {
            m_pipeline.RunCollect();
        }
        
        void OnDestroy()
        {
            PipelineSingleton.Dispose();
            EntityPool.Dispose();
            FragmentPool.Dispose();
        }
    };
}
