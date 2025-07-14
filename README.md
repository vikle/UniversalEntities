# Lightweight and easy-to-use ECS framework for Unity

## ⚡︎ Main Features
* QoL focused.
* Marker oriented.
* No code generation.
* Optimized for IL2CPP.

## 📝 Key Points
* `Entities` are с# classes, which allows to get components with minimal code.
* `Components` are с# classes too, which allows to get data with minimal code.
* `Filters` are disigned to a `foreach` loop to enumerate entities.
* All entities and components are created as needed and then reused through the `object pool`.
* Interfaces for `systems` are designed for `Update`, `FixedUpdate`, and `LateUpdate` Unity events.
* There are also interfaces for handling entities after creation and before destruction.
* `MonoBehaviour` objects can be integrated into the `Pipeline` via special `Bakers`.
* Compatible with `Unity 2019.4` and above, with `c# 7` and above, with `.net standard 2.0` and above.

## ❗️ ATTENTION
> Remember, use the `DEBUG` versions of the builds for development and the `RELEASE` versions of the builds for releases. All internal checks and exceptions will only work in the `DEBUG` versions and will be removed to improve performance in the `RELEASE` versions.

> This framework is **not thread-safe** and will never be! If you need multithreading, you must implement it yourself and integrate synchronization as an ecs system.

> [This is only one official working version.](https://github.com/vikle/UniversalEntities.git) All another versions are unofficial clones and can be contains unknown third-party code. Using these sources is strongly discouraged and should be done at your own risk.

## 📖 Table of Contents
* [Setup](#-setup)
* [Starter](#-starter)
* [Bootstrap](#-bootstrap)
* [Entity](#-entity)
* [Component](#-component)
* [Event](#-event)
* [Promise](#-promise)
* [Filter](#-filter)
* [System](#-system)
* [Pipeline](#-pipeline)
* [Actor](#-actor)
* [Bakers](#-bakers)

## 📃 Setup
Open the `Window -> Package Manager`, select the `Add package from git URL...` and paste [https://github.com/vikle/UniversalEntities.git](https://github.com/vikle/UniversalEntities.git)

## 📃 Starter
Create the Starter from `Tools -> Universal Entities -> Create Starter`.

The Starter (c# name is `UniversalEntitiesStarter`) is inheritor of `MonoBehaviour` designed to bootstrap pipeline and initialize engines.

```
Keep Alive - move this game object to DontDestroyOnLoad scene
Bootstrap - the reference for your own custom bootstrap
```

## 📃 Bootstrap
Attach this script on any `GameObject` and asssign it to Bootstrap field on the `Starter`

The Bootstrap (c# name is `UniversalEntitiesBootstrap`) is inheritor of `MonoBehaviour` designed to bind systems, promises, events into `Pipeline` to calls from `engines`.
```c#
public sealed class CustomBootstrap : UniversalEntitiesBootstrap
{
    public override void OnBootstrap(Pipeline pipeline)
    {
        pipeline
            .BindSystem<System01>()
            .BindSystem<System02>()
            .BindSystem<System03>()
            .BindSystem<System04>()
            .BindSystem<System05>()
            ;

        pipeline
            .BindPromise<Promise01>()
            .BindPromise<Promise02>()
            .BindPromise<Promise03>()
            ;

        pipeline
            .BindEvent<Event01>()
            .BindEvent<Event02>()
            ;
    }
};
```

## 📃 Entity
Reference type object. Allocated if needed or reused from pool. Contains references of components.
```c#
var pipeline = PipelineSingleton.Get;
var entity = pipeline.CreateEntity();

entity.AddComponent<Health>();

// Optionally, calls all inheritors of IEntityInitializeSystem
entity.Initialize();

var health = entity.GetComponent<Health>();
health.value++;

if (entity.IsAlive)
{
    entity.Destroy();
}
```

## 📃 Component
A datatype container that can be added to entities or removed from its.
Also uses as markers for filtering entities.
Reference type object. 
Allocated if needed or reused from pool. 

### Components Declaring
* IComponent
```c#
// Simple component
public sealed class Health : IComponent
{
    public float value;
};

entity.AddComponent<Health>();
```
* IResettableComponent
```c#
// Have special method, then called after component added to entity.
public sealed class Health : IResettableComponent
{
    public float value;

    void IResettableComponent.OnReset()
    {
       value = 1f;
    } 
};
```
* IUnmanagedComponent
```c#
// This object is not returns to internal object pool, after entity destroyed.
// That convenient for direct access to MonoBehaviour objects, if needed.
// Recommend using this for read only.
public sealed class Descriptor : MonoBehaviour, IUnmanagedComponent
{
    public float maxArmor;
    public float maxHealth;
};

Descriptor _descriptor;
entity.AddComponent(_descriptor);
```
* ObjectRef<T> where T : class
```c#
// ObjectRef is designed for access to internal Unity objects from entity.
entity.AddComponent<ObjectRef<Transform>>().Target = transform;

var transform = entity.GetComponent<ObjectRef<Transform>>().Target;
transform.position = Vector3.zero;
transform.rotation = Quaternion.identity;

Cast<TCast>() - Cast Target type to inheritor type, returns exception on failure.
TryCast(out TCast) - Safe cast Target type to inheritor type, returns false and null refference on failure.
```
* ValueRef<T> where T : unmanaged
```c#
// ValueRef is designed for access to value type fields from entity.
entity.AddComponent<ValueRef<MyStruct>>().Assign(ref myStructField);

var myStructFieldRef = entity.GetComponent<ValueRef<MyStruct>>();
myStructFieldRef.IsValid - check pointer
myStructFieldRef.ValueRO - read only ref
myStructFieldRef.ValueRW - read write ref
```
#### API
```c#
entity.HasComponent<ComponentType>(); // Returns true if entity has it 
entity.AddComponent<ComponentType>();
entity.AddComponent(unmanagedComponentInstance);
entity.GetComponent<ComponentType>();
entity.RemoveComponent<ComponentType>();
```

## 📃 Event
A single-frame special object (entity with single component) that contains main component.
The created entity with the promise component will exist to end of current frame.
It is important to consider the execution order of systems when calling them.

### Event Declaring
```c#
public sealed class CharacterSpawnedEvent : IEvent
{ 
     public Entity character;
};
```
### Event Calling
```c#
var spawnEvent = pipeline.Trigger<CharacterSpawnedEvent>();
spawnEvent.character = entity;
```
### Event Filtering
```c#
m_eventFilter = pipeline.Query.With<CharacterSpawnedEvent>().Build();
```
### Event Handling
```c#
foreach (var eventEntity in m_eventFilter)
{
     var evt = eventEntity.GetComponent<CharacterSpawnedEvent>();

     // ... handle system code
}
```
### Event Collecting
Internal system auto collect it after late update.
```c#
pipeline.BindEvent<CharacterSpawnedEvent>();
```

## 📃 Promise
A multiframe special object (entity with single component) that contains main component with its own state. 
Initially `pending` (waiting), then one of: `fulfilled` (completed successfully) or `rejected` (completed with an error).
The state is changed manually in any of the systems, depending on whether the goal was completed or not.
The created entity with the promise component will exist until the state changes. 
It is important to consider the execution order of systems when changing the state, if the state needs to be read in other systems.

### Promise Declaring
```c#
public sealed class SpawnCharacterPromise : IPromise
{
     public EPromiseState State { get; set; }
     public GameObject prefab;
     public Vector3 position;
     public Quaternion rotation;
};
```
### Promise Calling
```c#
var spawnPromise = pipeline.Then<SpawnCharacterPromise>();
spawnPromise.prefab = characterPrefab;
spawnPromise.position = spawnPoint.transform.position;
spawnPromise.rotation = spawnPoint.transform.rotation;
```
### Promise Filtering
```c#
m_promiseFilter = pipeline.Query.With<SpawnCharacterPromise>().Build();
```
### Promise Handling
```c#
foreach (var promiseEntity in m_promiseFilter)
{
     var promise = promiseEntity.GetComponent<SpawnCharacterPromise>();

     // ... handle system code

     // Desired result
     if (isSuccess)
     {
         promise.State = EPromiseState.Fulfilled;
         // or
         promiseEntity.MarkFulfilled<SpawnCharacterPromise>();
     }
     else
     {
         promise.State = EPromiseState.Rejected;
         // or
         promiseEntity.MarkRejected<SpawnCharacterPromise>();
     }
}
```
### Promise Collecting
Internal system auto collect it after late update.
```c#
pipeline.BindPromise<SpawnCharacterPromise>();
```

## 📃 Filter
This is designed as iterator of entities which has marked components.
```c#
pipeline.Query.With<CharacterMarker>().Build();
pipeline.Query.With<SpawnCharacterPromise>().Build();
pipeline.Query.With<CharacterSpawnedEvent>().Build();
```
All filters are updated automatically as soon as components are added to or removed to entities. 
However, this approach can create unnecessary overhead when there are many manipulations of components. 
In such cases, be useful to disable automatic filters updating and call then manually after.
**Highly recommended use this functions with extreme caution to avoid a bugs.**

```c#
pipeline.IsFiltersUpdatingEnabled; - read only status of filters updating.
pipeline.DisableFiltersUpdating(); - sets the status on false.
pipeline.EnableFiltersUpdating(); - sets the status on true.
pipeline.EnableFiltersUpdatingAndCallUpdate(Entity entity); - sets the status on true and call ForceUpdateFilters.
pipeline.UpdateFiltersIfEnabled(Entity entity); - call ForceUpdateFilters if status set true.
pipeline.ForceUpdateFilters(Entity entity); - update a filters and ignoring the status.
```
## 📃 System
Any system is part of separated logic to iterate and handle entities which has marked components. 
### Declaring
```c#
public sealed class SpawnCharacterSystem : IUpdateSystem
{
    readonly Filter m_filter;

    [Preserve]public SpawnCharacterSystem(Pipeline pipeline)
    {
        m_filter = pipeline.Query.With<SpawnCharacterPromise>().Build();
    }

    public void OnUpdate(Pipeline pipeline)
    {
        // Best practice for optimazation
        if (m_filter.IsEmpty) return;

        foreach (var promiseEntity in m_filter)
        {
            var promise = promiseEntity.GetComponent<SpawnCharacterPromise>();
            promise.State = EPromiseState.Fulfilled;

            var prefabClone = GameObjectPool.Instantiate(request.prefab, request.position, request.rotation);
            var actor = prefabClone.GetComponent<EntityActor>();

            // Calls for initialize entity and call bakers
            actor.InitEntity();

            var characterEntity = actor.EntityRef;

            var spawnEvent = pipeline.Trigger<CharacterSpawnedEvent>();
            spawnEvent.character = characterEntity;
        }
    }
};
```
### Binding
```c#
pipeline.BindSystem<SpawnCharacterSystem>();
```
### Systems Examples
* IAwakeSystem
```c#
// Execute on standard event, but after bootstrap and init pipeline
// DefaultExecutionOrder(-100)
// Caller: UniversalEntitiesStarter
public sealed class AwakeSystem : IAwakeSystem
{
    [Preserve]public AwakeSystem(Pipeline pipeline) { }
    public void OnAwake(Pipeline pipeline) { }
};
```
* IStartSystem
```c#
// Execute on standard event
// DefaultExecutionOrder(-50)
// Caller: UniversalEntitiesEngine
public sealed class StartSystem : IStartSystem
{
    [Preserve]public StartSystem(Pipeline pipeline) { }
    public void OnStart(Pipeline pipeline) { }
};
```
* IFixedUpdateSystem
```c#
// Execute on standard event
// DefaultExecutionOrder(-50)
// Caller: UniversalEntitiesEngine
public sealed class FixedUpdateSystem : IFixedUpdateSystem
{
    [Preserve]public FixedUpdateSystem(Pipeline pipeline) { }
    public void OnFixedUpdate(Pipeline pipeline) { }
};
```
* IUpdateSystem
```c#
// Execute on standard event
// DefaultExecutionOrder(-50)
// Caller: UniversalEntitiesEngine
public sealed class UpdateSystem : IUpdateSystem
{
    [Preserve]public UpdateSystem(Pipeline pipeline) { }
    public void OnUpdate(Pipeline pipeline) { }
};
```
* ILateUpdateSystem
```c#
// Execute on standard event
// DefaultExecutionOrder(-50)
// Caller: UniversalEntitiesEngine
public sealed class LateUpdateSystem : ILateUpdateSystem
{
    [Preserve]public LateUpdateSystem(Pipeline pipeline) { }
    public void OnLateUpdate(Pipeline pipeline) { }
};
```
* ICollectSystem
```c#
// Execute after standard LateUpdate event
// DefaultExecutionOrder(100)
// Caller: UniversalEntitiesCollector
public sealed class CollectSystem : ICollectSystem
{
    [Preserve]public CollectSystem(Pipeline pipeline) { }
    public void OnCollect(Pipeline pipeline) { }
};
```
* IEntityInitializeSystem
```c#
// Execute after call entity.Initialize(); if entity IsAlive is false.
public sealed class EntityInitializeSystem : IEntityInitializeSystem
{
    [Preserve]public EntityInitializeSystem(Pipeline pipeline) { }

    // Execute after Baker if actor attached to entity.
    public void OnAfterEntityCreated(Pipeline pipeline, Entity entity)
    {
        if (!entity.HasComponent<EntityMarker>()) return;

        // ... initialize entity logic
    }
};
```
* IEntityInitializeSystem
```c#
// Execute after call entity.Destroy(); if entity IsAlive is true.
public sealed class EntityTerminateSystem : IEntityInitializeSystem
{
    [Preserve]public EntityTerminateSystem(Pipeline pipeline) { }

    // Execute after Baker if actor attached to entity.
    public void OnBeforeEntityDestroyed(Pipeline pipeline, Entity entity)
    {
        if (!entity.HasComponent<EntityMarker>()) return;

        // ... dispose entity logic
    }
};
```
#### Useful API
* TimeData - contains unity time data written before systems execution.
```c#
TimeData.Time
TimeData.UnscaledTime
TimeData.DeltaTime
TimeData.UnscaledDeltaTime
TimeData.FixedTime
TimeData.FixedUnscaledTime
TimeData.FixedDeltaTime
TimeData.FixedUnscaledDeltaTime
```

## 📃 Pipeline
This is a container for all entities. In other ECS frameworks it's known as "world" or "context".
```c#
var pipeline = PipelineSingleton.Get;

var entity = pipeline.CreateEntity();
int entityCount = pipeline.EntityCount;

pipeline.BindSystem<T>();
pipeline.BindPromise<T>();
pipeline.BindEvent<T>();
```

## 📃 Actor
The Actor (c# name is `EntityActor`) is inheritor of `MonoBehaviour` designed to attach into `GameObject` and manage the lifecycle of an entity attached to this `GameObject`.

* Data Component - Distributes GameObject data.
```c#
// Single Entity Actor Data - include data in single component
var data = entity.AddComponent<EntityActorData>();
data.gameObject = gameObject;
data.transform = transform;
data.actor = this;

// Separated Object Ref - include data in separated components
entity.AddComponent<ObjectRef<GameObject>>().Target = gameObject;
entity.AddComponent<ObjectRef<Transform>>().Target = transform;
entity.AddComponent<ObjectRef<EntityActor>>().Target = this;

// Excluded - Don't include data for this GameObject.
```
* Optimize Filters Updating - disable automatic filters updating on full entity initialize path. 
Warning, If you create entities with it's set true, necessarily force update a filters for each entity manually.

## 📃 Bakers
The Baker (c# name is `EntityActorBaker`) is inheritor of `MonoBehaviour` designed to attach `Unity objects` references to `entity` as `components` for access its from `systems`.
A custom bakers (yes, its can be multiple) must be attached to the same object as `EntityActor`.

Yes, it can be used as alternative of `Entity Initialize System` and `Entity Terminate System`, but its not recommended. 
Because it contradicts of ECS concept. Best practice be separate logic of Initialization and Termination into systems.

### Declaring
```c#
public sealed class CustomBaker : EntityActorBaker
{
    // Execute before IEntityInitializeSystem
    public override void OnAfterEntityCreated(Pipeline pipeline, Entity entity, EntityActor actor)
    {
        entity.AddComponent<EntityMarker>();
    }

    // Execute before IEntityTerminateSystem
    public override void OnBeforeEntityDestroyed(Pipeline pipeline, Entity entity, EntityActor actor) { }
}
```
