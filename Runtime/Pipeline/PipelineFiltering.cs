using System.Runtime.CompilerServices;

namespace UniversalEntities
{
    partial class Pipeline
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void EnsureFiltersCapacity(int capacity)
        {
            // var filters = m_filtersBuffer;
            //
            // for (int i = 0, i_max = m_filtersCount; i < i_max; i++)
            // {
            //     filters[i].EnsureCapacity(capacity);
            // }
        }
        
        
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void RemoveEntityFromAllFilters(Entity entity)
        {
            // var filters = m_filtersBuffer;
            //
            // for (int i = 0, i_max = m_filtersCount; i < i_max; i++)
            // {
            //     filters[i].RemoveEntity(entity);
            // }
        }
    };
}
