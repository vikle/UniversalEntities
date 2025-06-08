using System.Runtime.CompilerServices;

#if ENABLE_IL2CPP
using Unity.IL2CPP.CompilerServices;
#endif

namespace UniversalEntities
{
#if ENABLE_IL2CPP
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
#endif
    public ref struct FilterBuilder
    {
        BitMask m_mask;
        readonly Pipeline m_pipeline;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal FilterBuilder(Pipeline pipeline)
        {
            m_mask = default;
            m_pipeline = pipeline;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FilterBuilder With<T>() where T : class, IFragment
        {
            int type_index = FragmentTypeIndex<T>.Index;
            m_mask.Set(type_index);
            return this;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Filter Build()
        {
            ref readonly var mask = ref m_mask;
            var pipeline = m_pipeline;
            
            if (pipeline.TryGetFilter(in mask, out var filter))
            {
                return filter;
            }

            filter = pipeline.AllocateFilter(in mask);

            foreach (var entity in pipeline)
            {
                if (entity.Mask.Has(in mask))
                {
                    filter.AddEntity(entity.Id);
                }
            }
            
            return filter;
        }
    };
}
