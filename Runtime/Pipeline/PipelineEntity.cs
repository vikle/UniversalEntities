using System.Runtime.CompilerServices;

namespace UniversalEntities
{
    partial class Pipeline
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Entity CreateEntity()
        {
            var entity = EntityPool.Get();
            AddEntity(entity);
            return entity;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void DestroyEntity(Entity entity)
        {
            if (!entity.IsAlive) return;
            
            RemoveEntity(entity);
            
            entity.Dispose();
            EntityPool.Release(entity);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void AddEntity(Entity entity)
        {
            entity.Init(m_denseCount);
            
            for (int i = 0, i_max = m_entityInitializeSystems.Length; i < i_max; i++)
            {
                m_entityInitializeSystems[i].OnAfterEntityCreated(this, entity);
            }
            
            ArrayTool.Push(ref m_denseEntities, ref m_denseCount, entity);
        }
    
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void RemoveEntity(Entity entity)
        {
            int entity_index = entity.Index;
            int last_index = --m_denseCount;
            
            var entities = m_denseEntities;

            if (entity_index < last_index)
            {
                var last_entity = entities[last_index];
                entities[entity_index] = last_entity;
                last_entity.Index = entity_index;
            }
            
            entities[last_index] = null;
            
            for (int i = 0, i_max = m_entityTerminateSystems.Length; i < i_max; i++)
            {
                m_entityTerminateSystems[i].OnBeforeEntityDestroyed(this, entity);
            }
        }
    };
}
