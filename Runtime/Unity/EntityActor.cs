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
        public enum EDataComponent : byte
        {
            SingleEntityActorData,
            SeparatedObjectRef,
            Excluded
        };
        
        public EDataComponent dataComponent;
        
        public Entity EntityRef
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]get; 
            [MethodImpl(MethodImplOptions.AggressiveInlining)]private set;
        }
        
        EntityActorBaker[] m_backers;

        Pipeline m_pipeline;

        void Awake()
        {
            m_pipeline = UniversalEntitiesEngine.PipelineInstance;
            InitBakers();
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
            
            InitBakers();
            
            var entity = m_pipeline.CreateEntity();

            switch (dataComponent)
            {
                case EDataComponent.SingleEntityActorData:
                    var data = entity.AddComponent<EntityActorData>();
                    data.gameObject = gameObject;
                    data.transform = transform;
                    data.actor = this;
                    break;
                case EDataComponent.SeparatedObjectRef: 
                    entity.AddComponent<ObjectRef<GameObject>>().Target = gameObject;
                    entity.AddComponent<ObjectRef<Transform>>().Target = transform;
                    entity.AddComponent<ObjectRef<EntityActor>>().Target = this;
                    break;
                default: break;
            }
            
            for (int i = 0, i_max = m_backers.Length; i < i_max; i++)
            {
                m_backers[i].OnAfterEntityCreated(m_pipeline, EntityRef, this);
            }
            
            entity.Initialize();

            EntityRef = entity;
        }

        public void DisposeEntity()
        {
            var entity = EntityRef;
            
            if (entity == null) return;
            
            for (int i = 0, i_max = m_backers.Length; i < i_max; i++)
            {
                m_backers[i].OnBeforeEntityDestroyed(m_pipeline, EntityRef, this);
            }
            
            m_pipeline.DestroyEntity(entity);
            
            EntityRef = null;
        }

        private void InitBakers()
        {
            if (m_backers == null)
            {
                m_backers = GetComponents<EntityActorBaker>();
            }
        }
    };
}
