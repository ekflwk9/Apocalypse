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
    protected bool IsInit = false;
    protected EntityStateMachine StateMachine;
    protected Entity entity;
    int HashAnim;
    public void SetOwner(Entity _Entity, EntityStateMachine _StateMachine)
    {
        entity = _Entity;
        StateMachine = _StateMachine;
    }

    public void SetAnimation(int _HashAnim)
    {
        HashAnim = _HashAnim;
        entity._animator.Play(HashAnim);
    }

    public bool IsAnimationEnd()
    {
        var stateInfo = entity._animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.normalizedTime >= 1f)
        {
            return true;
        }
        return false;
    }

    public virtual void Enter() { }
    public virtual void Update() { }
    public virtual void Exit() { }
}
