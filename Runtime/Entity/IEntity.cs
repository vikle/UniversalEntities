using System;

namespace UniversalEntities
{
    public interface IEntity : IDisposable
    {
        bool Has<T>() where T : class, IFragment;
        bool TryGet<T>(out T fragment) where T : class, IFragment;
        T Then<T>() where T : class, IPromise, new();
        void Reject<T>() where T : class, IPromise;
        T Add<T>() where T : class, IFragment, new();
        void Add<T>(T instance) where T : class, IFragment;
        T Trigger<T>() where T : class, IEvent, new();
        void Remove<T>() where T : class, IFragment;
        void Remove<T>(T instance) where T : class, IFragment;
        void Remove(EntityActorComponent instance);
    };
}
