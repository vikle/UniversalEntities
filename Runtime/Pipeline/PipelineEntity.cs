using System.Runtime.CompilerServices;

namespace UniversalEntities
{
    partial class Pipeline
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Entity CreateEntity()
        {
            int entity_id = m_sparseSet.Alloc();
            
            var entity = EntityPool.Get();
            
            entity.Construct(this, entity_id);
            
            ArrayTool.EnsureCapacity(ref m_sparseEntities, entity_id);
            m_sparseEntities[entity_id] = entity;

            EnsureFiltersCapacity(entity_id);
            
            return entity;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void InitializeEntity(Entity entity)
        {
            RunAfterEntityCreated(entity);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void DestroyEntity(Entity entity)
        {
            if (!entity.IsAlive) return;
            
            int entity_id = entity.Id;
            
            RemoveEntityFromAllFilters(entity_id);
            RemoveEntity(entity_id);

            RunBeforeEntityDestroyed(entity);

            entity.Dispose();
            EntityPool.Release(entity);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void RemoveEntity(int entityId)
        {
            m_sparseSet.Free(entityId);
            m_sparseEntities[entityId] = null;
        }
    };
}
