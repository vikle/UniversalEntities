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
    public abstract class EntityActorBaker : MonoBehaviour
    {
        public abstract void OnAfterEntityCreated(Pipeline pipeline, Entity entity, EntityActor actor);
        
        public abstract void OnBeforeEntityDestroyed(Pipeline pipeline, Entity entity, EntityActor actor);
    };
}
