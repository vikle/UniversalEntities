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
    public abstract class EntityActor : MonoBehaviour
    {
        public Entity EntityRef
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]get; 
            [MethodImpl(MethodImplOptions.AggressiveInlining)]private set;
        }
        
        EntityActorComponent[] m_attachedComponents;

        Pipeline m_pipeline;

        void Awake()
        {
            m_pipeline = UniversalEntitiesEngine.PipelineInstance;
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
            
            var entity = m_pipeline.CreateEntity();

            entity.AddComponent<ObjectRef<GameObject>>().Target = gameObject;
            entity.AddComponent<ObjectRef<Transform>>().Target = transform;
            entity.AddComponent<ObjectRef<EntityActor>>().Target = this;
            
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

            m_pipeline.DestroyEntity(entity);
            
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
