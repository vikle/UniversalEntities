using System.Runtime.CompilerServices;

namespace UniversalEntities
{
    partial class Pipeline
    {
        public FilterBuilder Query
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new FilterBuilder(this);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void EnsureFiltersCapacity(int capacity)
        {
            var filters = m_filtersBuffer;
            
            for (int i = 0, i_max = m_filtersCount; i < i_max; i++)
            {
                filters[i].EnsureCapacity(capacity);
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal bool TryGetFilter(in BitMask mask, out Filter filter)
        {
            return m_filtersMap.TryGetValue(mask, out filter);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal Filter AllocateFilter(in BitMask mask)
        {
            var filter = new Filter(this, in mask);
            ArrayTool.Push(ref m_filtersBuffer, ref m_filtersCount, filter);
            m_filtersMap[mask] = filter;
            return filter;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void OnEntityFragmentAdded(Entity entity)
        {
            if (AutoUpdateFilters)
            {
                UpdateFilters(entity);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void OnEntityFragmentRemoved(Entity entity)
        {
            if (entity.Mask.IsEmpty)
            {
                DestroyEntity(entity);
                return;
            }

            if (AutoUpdateFilters)
            {
                UpdateFilters(entity);
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void UpdateFilters(Entity entity)
        {
            int entity_id = entity.Id;
            ref readonly var entity_mask = ref entity.Mask;
            var filters_buffer = m_filtersBuffer;

            for (int i = 0, i_max = m_filtersCount; i < i_max; i++)
            {
                var filter = filters_buffer[i];

                if (filter.IsCompatibleWith(entity_mask))
                {
                    filter.AddEntity(entity_id);
                }
                else if (filter.Contains(entity_id))
                {
                    filter.RemoveEntity(entity_id);
                }
            }
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void RemoveEntityFromAllFilters(int entityId)
        {
            var filters = m_filtersBuffer;
            
            for (int i = 0, i_max = m_filtersCount; i < i_max; i++)
            {
                filters[i].RemoveEntity(entityId);
            }
        }
    };
}
