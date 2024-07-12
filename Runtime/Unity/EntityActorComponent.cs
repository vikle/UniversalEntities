using System.Runtime.CompilerServices;
using UnityEngine;

namespace UniversalEntities
{
    [RequireComponent(typeof(EntityActor))]
    public abstract class EntityActorComponent : MonoBehaviour, IComponent
    {
        [SerializeField]EntityActor m_actor;
        public EntityActor Actor
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => m_actor;
        }
        
#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            if (m_actor == null) m_actor = GetComponent<EntityActor>();
        }
#endif
    };
}
