using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EntityEnum
{
    None = -1,
    Idle,
    Walk,
    Run,
    Attack,
    Die,
}

public abstract class EntityState
{
    protected Entity entity;
    public void SetOwner(Entity _Entity)
    {
        entity = _Entity;
    }

    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }
}
