using System;

namespace UniversalEntities
{
    public interface IEntity : IDisposable
    {
        bool HasComponent<T>() where T : class, IFragment;
        bool TryGetComponent<T>(out T fragment) where T : class, IFragment;
        T Then<T>() where T : class, IPromise, new();
        void Mark<T>(EPromiseState newState) where T : class, IPromise;
        T AddComponent<T>() where T : class, IFragment, new();
        void AddComponent<T>(T instance) where T : class, IFragment;
        T Trigger<T>() where T : class, IEvent, new();
        void RemoveComponent<T>() where T : class, IFragment;
        void RemoveComponent<T>(T instance) where T : class, IFragment;
        void RemoveComponent(EntityActorComponent instance);
    };
}
