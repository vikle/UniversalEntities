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
            
            entity.Init(this, entity_id);
            
            ArrayTool.EnsureCapacity(ref m_sparseEntities, entity_id);
            m_sparseEntities[entity_id] = entity;
            
            return entity;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DestroyEntity(Entity entity)
        {
            if (!entity.IsAlive) return;
            
            RunBeforeEntityDestroyed(entity);
            
            RemoveEntity(entity);
            
            RemoveEntityFromAllFilters(entity);
            
            entity.Dispose();
            EntityPool.Release(entity);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void RemoveEntity(Entity entity)
        {
            int entity_id = entity.Id;
            m_sparseEntities[entity_id] = null;
            m_sparseSet.Free(entity_id);
        }
    };
}
