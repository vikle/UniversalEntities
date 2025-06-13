#if ENABLE_IL2CPP
using Unity.IL2CPP.CompilerServices;
#endif

namespace UniversalEntities
{
#if ENABLE_IL2CPP
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
#endif
    public sealed class EventCollector<T> : IUpdateSystem where T : class, IEvent
    {
        readonly Filter m_filter;

#if UNITY_2020_3_OR_NEWER
        [UnityEngine.Scripting.Preserve]
#endif
        public EventCollector(Pipeline pipeline)
        {
            m_filter = pipeline.Query.With<T>().Build();
        }
        
        public void OnUpdate(Pipeline pipeline)
        {
            if (m_filter.IsEmpty) return;
            
            for (m_filter.Begin(); m_filter.TryIterate(out var entity);)
            {
                pipeline.DestroyEntity(entity);
            }
        }
    };
}
