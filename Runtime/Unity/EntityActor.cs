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
        public bool optimizeFiltersUpdating = true;
        
        public Entity EntityRef
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]get; 
            [MethodImpl(MethodImplOptions.AggressiveInlining)]private set;
        }
        
        EntityActorBaker[] m_backers;

        void Awake()
        {
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
            if (!PipelineSingleton.IsAlive) return;
            
            if (EntityRef != null) return;

            InitBakers();

            var pipeline = PipelineSingleton.Get;
            var entity = pipeline.CreateEntity();

            bool optimize_filters = optimizeFiltersUpdating;
            
            if (optimize_filters)
            {
                pipeline.DisableFiltersUpdating();
            }
            
            switch (dataComponent)
            {
                case EDataComponent.SingleEntityActorData:
                    var data = entity.AddComponent<EntityActorData>();
                    data.gameObject = gameObject;
                    data.transform = transform;
                    data.actor = this;
                    break;
                
                case EDataComponent.SeparatedObjectRef: 
                    if (!optimize_filters) pipeline.DisableFiltersUpdating();
                    entity.AddComponent<ObjectRef<GameObject>>().Target = gameObject;
                    entity.AddComponent<ObjectRef<Transform>>().Target = transform;
                    entity.AddComponent<ObjectRef<EntityActor>>().Target = this;
                    if (!optimize_filters) pipeline.EnableFiltersUpdatingAndCallUpdate(entity);
                    break;
                default: break;
            }
            
            for (int i = 0, i_max = m_backers.Length; i < i_max; i++)
            {
                m_backers[i].OnAfterEntityCreated(pipeline, entity, this);
            }
            
            entity.Initialize();

            if (optimize_filters)
            {
                pipeline.EnableFiltersUpdatingAndCallUpdate(entity);
            }
            
            EntityRef = entity;
        }

        public void DisposeEntity()
        {
            if (!PipelineSingleton.IsAlive) return;
            
            var entity = EntityRef;
            
            if (entity == null) return;
            
            var pipeline = PipelineSingleton.Get;
            
            for (int i = 0, i_max = m_backers.Length; i < i_max; i++)
            {
                m_backers[i].OnBeforeEntityDestroyed(pipeline, entity, this);
            }
            
            pipeline.DestroyEntity(entity);
            
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
