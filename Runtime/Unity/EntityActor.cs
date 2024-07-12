using System.Runtime.CompilerServices;
using UnityEngine;

namespace UniversalEntities
{
    [DisallowMultipleComponent]
    public sealed class EntityActor : MonoBehaviour
    {
        public IEntity Entity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]get; 
            [MethodImpl(MethodImplOptions.AggressiveInlining)]private set;
        }
        
        EntityActorComponent[] m_attachedComponents;

        void Awake()
        {
            InitAttachedComponents();
        }

        void OnEnable()
        {
            InitEntity();
        }

        void OnDisable()
        {
            DisposeEntity();
        }

        public void InitEntity()
        {
            if (Entity != null) return;
            
            InitAttachedComponents();
            
            var entity = ECSEngine.Context.CreateEntity<Entity>();
            
            for (int i = 0, i_max = m_attachedComponents.Length; i < i_max; i++)
            {
                var instance = m_attachedComponents[i];
                entity.Add(instance);
            }

            Entity = entity;
        }

        public void DisposeEntity()
        {
            var entity = Entity;
            
            if (entity == null) return;

            ECSEngine.Context.DestroyEntity(entity);
            
            for (int i = 0, i_max = m_attachedComponents.Length; i < i_max; i++)
            {
                var instance = m_attachedComponents[i];
                entity.Remove(instance);
            }
            
            Entity = null;
        }

        private void InitAttachedComponents()
        {
            m_attachedComponents ??= GetComponents<EntityActorComponent>();
        }
    };
}
