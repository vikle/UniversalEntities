using System.Runtime.CompilerServices;

#if ENABLE_IL2CPP
using Unity.IL2CPP.CompilerServices;
#endif

namespace UniversalEntities
{
#if ENABLE_IL2CPP
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
#endif
    public sealed class ObjectRef<T> : IComponent where T : class
    {
        public T Target;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public TCast Cast<TCast>() where TCast : T
        {
            return (TCast)Target;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryCast<TCast>(out TCast value) where TCast : T
        {
            if (Target is TCast casted)
            {
                value = casted;
                return true;
            }

            value = default;
            return false;
        }
    };
}
