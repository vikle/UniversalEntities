using UnityEngine;
using UniversalEntities;

public sealed class EntityActorData : IComponent
{
    public GameObject gameObject;
    public Transform transform;
    public EntityActor actor;
};
