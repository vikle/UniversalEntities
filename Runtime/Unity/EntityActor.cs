using System.Runtime.CompilerServices;
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
            
            var entity = UniversalEntitiesEngine.GetContext.CreateEntity<Entity>();
            
            for (int i = 0, i_max = m_attachedComponents.Length; i < i_max; i++)
            {
                var instance = m_attachedComponents[i];
                entity.AddComponent(instance);
            }

            Entity = entity;
        }

        public void DisposeEntity()
        {
            var entity = Entity;
            
            if (entity == null) return;

            UniversalEntitiesEngine.GetContext.DestroyEntity(entity);
            
            for (int i = 0, i_max = m_attachedComponents.Length; i < i_max; i++)
            {
                var instance = m_attachedComponents[i];
                entity.RemoveComponent(instance);
            }
            
            Entity = null;
        }

        private void InitAttachedComponents()
        {
            if (m_attachedComponents == null)
            {
                m_attachedComponents = GetComponents<EntityActorComponent>();
            }
        }
    };
}
