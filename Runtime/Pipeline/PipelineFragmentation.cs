using System.Runtime.CompilerServices;

namespace UniversalEntities
{
    partial class Pipeline
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Trigger<T>() where T : class, IEvent, new()
        {
            var entity = CreateEntity();
            return entity.AddComponent<T>();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Trigger<T>(out Entity entity) where T : class, IEvent, new()
        {
            entity = CreateEntity();
            return entity.AddComponent<T>();
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Then<T>() where T : class, IPromise, new()
        {
            var entity = CreateEntity();
            var promise = entity.AddComponent<T>();
            promise.State = EPromiseState.Pending;
            return promise;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Then<T>(out Entity entity) where T : class, IPromise, new()
        {
            entity = CreateEntity();
            var promise = entity.AddComponent<T>();
            promise.State = EPromiseState.Pending;
            return promise;
        }
    };
}
