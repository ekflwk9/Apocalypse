using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public enum EntityEnum
{
    None = -1,
    Idle,
    Walk,
    Hearing,
    Run,
    Attack,
    Hit,
    Hurt,
    Dying,
    Die,
}

public abstract class EntityState
{
    protected bool IsInit = false;
    protected EntityStateMachine StateMachine;
    protected Entity entity;
    public EntityEnum _EntityEnum { get; protected set; }
    int HashAnim;
    public virtual void SetOwner(Entity _Entity, EntityStateMachine _StateMachine)
    {
        entity = _Entity;
        StateMachine = _StateMachine;
    }

    public void SetAnimation(int _HashAnim)
    {
        HashAnim = _HashAnim;
        entity._animator.Play(HashAnim, 0);
        entity._animator.Play(HashAnim, 1);
    }

    public void SetAnimationForce(int _HashAnim)
    {
        HashAnim = _HashAnim;
        entity._animator.Play(HashAnim, 0, 0f);
        entity._animator.Play(HashAnim, 1, 0f);
    }

    public void SetBottomAnimation(int _HashAnim)
    {
        HashAnim = _HashAnim;
        entity._animator.Play(HashAnim, 0);
    }

    public void SetUpperAnimation(int _HashAnim)
    {
        HashAnim = _HashAnim;
        entity._animator.Play(HashAnim, 1);
    }

    public void SetAnimationFade(int _HashAnim, float _Time)
    {
        HashAnim = _HashAnim;
        entity._animator.CrossFade(HashAnim, _Time, 0);
        entity._animator.CrossFade(HashAnim, _Time, 1);
    }


    public void SetBottomAnimationFade(int _HashAnim, float _Time)
    {
        HashAnim = _HashAnim;
        entity._animator.CrossFade(HashAnim, _Time, 0);
    }

    public void SetUpperAnimationFade(int _HashAnim, float _Time)
    {
        HashAnim = _HashAnim;
        entity._animator.CrossFade(HashAnim, _Time, 1);
    }

    public bool IsAnimationEnd(int _Layer = 0)
    {
        var stateInfo = entity._animator.GetCurrentAnimatorStateInfo(_Layer);
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
