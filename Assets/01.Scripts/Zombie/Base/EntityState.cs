using UnityEngine;

public static class AnimHash
{
    public static readonly int IdleHash = Animator.StringToHash("Idle");
    public static readonly int WalkHash = Animator.StringToHash("Walking");
    public static readonly int RunHash_1 = Animator.StringToHash("Run_1");
    public static readonly int RunHash_2 = Animator.StringToHash("Run_2");
    public static readonly int HitHash_1 = Animator.StringToHash("Hit_1");
    public static readonly int HitHash_2 = Animator.StringToHash("Hit_2");
    public static readonly int YellingHash = Animator.StringToHash("Yelling");
    public static readonly int SneakHash = Animator.StringToHash("Sneak");
    public static readonly int SneakMoveHash = Animator.StringToHash("SneakMove");
    public static readonly int AttackHash_1 = Animator.StringToHash("Attack_1");
    public static readonly int AttackHash_2 = Animator.StringToHash("Attack_2");
    public static readonly int HurtHash = Animator.StringToHash("Hurt");
    public static readonly int DieHash = Animator.StringToHash("Dying");
}
public enum EntityEnum
{
    None = -1,
    Idle,
    Walk,
    Hearing,
    Detect,
    Run,
    Yelling,
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

    protected void SetDirection()
    {
        Vector3 entityPos = entity.transform.position;
        Vector3 playerPos = Player.Instance.transform.position;
        Vector3 directionPlayer = (playerPos - entityPos).normalized;
        Quaternion LookDirection = Quaternion.LookRotation(directionPlayer);
        entity.transform.rotation = LookDirection;
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

    public void SetUpperAnimationForce(int _HashAnim)
    {
        HashAnim = _HashAnim;
        entity._animator.Play(HashAnim, 1, 0f);
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
