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
        public Entity EntityRef
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]get; 
            [MethodImpl(MethodImplOptions.AggressiveInlining)]private set;
        }
        
        EntityActorComponent[] m_attachedComponents;

        Context m_context;

        void Awake()
        {
            m_context = UniversalEntitiesEngine.GetContext;
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
            if (EntityRef != null) return;
            
            InitAttachedComponents();
            
            var entity = m_context.CreateEntity();
            
            for (int i = 0, i_max = m_attachedComponents.Length; i < i_max; i++)
            {
                var instance = m_attachedComponents[i];
                entity.AddComponent(instance);
            }

            EntityRef = entity;
        }

        public void DisposeEntity()
        {
            var entity = EntityRef;
            
            if (entity == null) return;

            m_context.DestroyEntity(entity);
            
            for (int i = 0, i_max = m_attachedComponents.Length; i < i_max; i++)
            {
                var instance = m_attachedComponents[i];
                // entity.RemoveComponent(instance);
            }
            
            EntityRef = null;
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
