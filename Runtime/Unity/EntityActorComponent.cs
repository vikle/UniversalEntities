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
