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
* [Setup](#-setup4)
* [Entity](#-setup)
  * [Initialize](#-setup3)
  * [Terminate](#-setup)
  * [Actor](#-setup)
  * [ActorBaker](#-setup)
* [Component](#-setup)
  * [Resettable](#-setup)
  * [Unmanaged](#-setup)
* [System](#-setup)
  * [Awake](#-setup)
  * [Start](#-setup)
  * [Fixed Update](#-setup)
  * [Update](#-setup)
  * [Late Update](#-setup)    
  * [Collect](#-setup)
* [Promise](#-setup)
* [Event](#-setup)
* [Filter](#-setup)
* [Pipeline](#-setup)
* [Starter](#-setup)
* [Bootstrap](#-setup)


## ✏️ Setup
Open the `Window -> Package Manager`, select the `Add package from git URL...` and paste [https://github.com/vikle/UniversalEntities.git](https://github.com/vikle/UniversalEntities.git)


## ✏️ Entity


## ✏️ Component




## ✏️ System



## ✏️ Promise

## ✏️ Event

## ✏️ Filter

## ✏️ Pipeline

## ✏️ Starter

## ✏️ Bootstrap

## ✏️ Setup

## ✏️ Setup



