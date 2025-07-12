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
* `MonoBehaviour` objects can be intergated into the `Pipeline` via special `Bakers`.
* Compatible with `Unity 2019.4` and above, with `c# 7` and above, with `.net standard 2.0` and above.

## ❗️ ATTENTION
> Remember, use the `DEBUG` versions of the builds for development and the `RELEASE` versions of the builds for releases. All internal checks and exceptions will only work in the `DEBUG` versions and will be removed to improve performance in the `RELEASE` versions.

> This framework is **not thread-safe** and will never be! If you need multithreading, you must implement it yourself and integrate synchronization as an ecs system.

> [This is only one official working version.](https://github.com/vikle/UniversalEntities.git) All another versions are unofficial clones and can be contains unknown third-party code. Using these sources is strongly discouraged and should be done at your own risk.

## 📖 Table of Contents
* [Setup](#-setup)
* [Prepare](#-setup)
* [Entity](#-setup)
  * [Actor](#-setup)
  * [ActorBaker](#-setup)
  * [Initialize](#-setup3)
  * [Terminate](#-setup)
* [Component](#-setup)
  * [Resettable](#-setup)
  * [Unmanaged](#-setup)
  * [ObjectRef<T>](#-setup)
* [Promise](#-setup)
* [Event](#-setup)
* [Filter](#-setup)
* [System](#-setup)
  * [Awake](#-setup)
  * [Start](#-setup)
  * [Fixed Update](#-setup)
  * [Update](#-setup)
  * [Late Update](#-setup)    
  * [Collect](#-setup)
* [Pipeline](#-setup)
* [Starter](#-setup)
* [Bootstrap](#-setup)


## ✏️ Setup
Open the `Window -> Package Manager`, select the `Add package from git URL...` and paste [https://github.com/vikle/UniversalEntities.git](https://github.com/vikle/UniversalEntities.git)

## ✏️ Prepare
Create the Starter from `Tools -> Universal Entities -> Create Starter`

## ✏️ Entity
Reference type object. Allocated if needed or reused from pool. Contains references of components.
```c#
var pipeline = PipelineSingleton.Get;
var entity = pipeline.CreateEntity();

entity.AddComponent<CharacterMarker>();

// Optionally, calls all inheritors of IEntityInitializeSystem
entity.Initialize();
```

## ✏️ Component
Reference type object. Allocated if needed or reused from pool. Store data and uses as markers for filtering entities.
```c#
public sealed class Health : IComponent
{
    public float value;
};
```
```c#
public sealed class Health : IResettableComponent
{
    public float value;

    // Called after component added
    void IResettableComponent.OnReset()
    {
       value = 1f;
    } 
};
```

## ✏️ Promise

```c#
public sealed class SpawnCharacterPromise : IPromise
{
     public EPromiseState State { get; set; }
     public GameObject prefab;
     public Vector3 position;
     public Quaternion rotation;
};

var spawnPromise = pipeline.Then<SpawnCharacterPromise>();
spawnPromise.prefab = characterPrefab;
spawnPromise.position = spawnPoint.transform.position;
spawnPromise.rotation = spawnPoint.transform.rotation;
```

## ✏️ Event

```c#
public sealed class CharacterSpawnedEvent : IEvent
{ 
     public Entity character;
};

var spawnEvent = pipeline.Trigger<CharacterSpawnedEvent>();
spawnEvent.character = entity;
```

## ✏️ Filter

```c#
pipeline.Query.With<CharacterMarker>().Build();
pipeline.Query.With<SpawnCharacterPromise>().Build();
pipeline.Query.With<CharacterSpawnedEvent>().Build();
```

## ✏️ System

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

## ✏️ Pipeline
```c#
// Pipeline
```

## ✏️ Starter

## ✏️ Bootstrap
```c#
// Bootstrap
```

## ✏️ Setup

## ✏️ Setup



