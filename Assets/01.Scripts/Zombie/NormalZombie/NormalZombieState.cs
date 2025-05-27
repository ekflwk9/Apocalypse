using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.VersionControl.Asset;
using static UnityEngine.EventSystems.EventTrigger;

public static class AnimHash
{
    public static readonly int IdleHash = Animator.StringToHash("Idle");
    public static readonly int WalkHash = Animator.StringToHash("Walking");
    public static readonly int RunHash_1 = Animator.StringToHash("Run_1");
    public static readonly int RunHash_2 = Animator.StringToHash("Run_2");
    public static readonly int HitHash_1 = Animator.StringToHash("Hit_1");
    public static readonly int HitHash_2 = Animator.StringToHash("Hit_2");
    public static readonly int AttackHash_1 = Animator.StringToHash("Attack_1");
    public static readonly int AttackHash_2 = Animator.StringToHash("Attack_2");
    public static readonly int HurtHash = Animator.StringToHash("Hurt");
    public static readonly int DieHash = Animator.StringToHash("Dying");
}


public class NormalZombieStateStruct : StateStruct
{
    public override void Init(EntityStateMachine _StateMachine)
    {
        IdleState idleState = new IdleState();
        WalkState walkState = new WalkState();
        RunState runState = new RunState();
        AttackState attackState = new AttackState();
        HitState hitState = new HitState();
        HurtState hurtState = new HurtState();
        DyingState dyingState = new DyingState();
        StateDictionary.Add(EntityEnum.Idle, idleState);
        StateDictionary.Add(EntityEnum.Walk, walkState);
        StateDictionary.Add(EntityEnum.Run, runState);
        StateDictionary.Add(EntityEnum.Attack, attackState);
        StateDictionary.Add(EntityEnum.Hit, hitState);
        StateDictionary.Add(EntityEnum.Hurt, hurtState);
        StateDictionary.Add(EntityEnum.Dying, dyingState);
        base.Init(_StateMachine);
    }
}

public class IdleState : EntityState
{
    public override void SetOwner(Entity _Entity, EntityStateMachine _StateMachine)
    {
        _EntityEnum = EntityEnum.Idle;
        base.SetOwner(_Entity, _StateMachine);
    }

    public override void Enter()
    {
        SetAnimation(AnimHash.IdleHash);
    }
    public override void Update()
    {
        if (true == IsAnimationEnd())
        {
            StateMachine.SetState(EntityEnum.Walk);
        }
    }
    public override void Exit()
    {
    }
}

public class WalkState : EntityState
{
    NavMeshAgent _NavMeshAgent;

    public override void SetOwner(Entity _Entity, EntityStateMachine _StateMachine)
    {
        _EntityEnum = EntityEnum.Walk;
        base.SetOwner(_Entity, _StateMachine);
    }

    public override void Enter()
    {
        if (true != IsInit)
        {
            _NavMeshAgent = entity.GetComponent<NavMeshAgent>();
            IsInit = true;
        }
        Vector3 TargetPos = NaviHelper.GetRandomNavMeshPosition(entity.transform.position, 30f);
        _NavMeshAgent.SetDestination(TargetPos);
        _NavMeshAgent.speed = entity.baseStatus.WalkSpeed;
        entity._animator.Play(AnimHash.WalkHash);
    }
    public override void Update()
    {
        if (true == NaviHelper.IsArrived(_NavMeshAgent, .1f))
        {
            StateMachine.SetState(EntityEnum.Idle);
        }
    }
    public override void Exit()
    {
    }
}

public class RunState : EntityState
{
    NavMeshAgent _NavMeshAgent;

    public override void SetOwner(Entity _Entity, EntityStateMachine _StateMachine)
    {
        _EntityEnum = EntityEnum.Run;
        base.SetOwner(_Entity, _StateMachine);
    }

    public override void Enter()
    {
        if (true != IsInit)
        {
            _NavMeshAgent = entity.GetComponent<NavMeshAgent>();
            IsInit = true;
        }

        if (entity.baseStatus.IsHalf() == true)
        {
            SetAnimation(AnimHash.RunHash_2);
            _NavMeshAgent.speed = entity.baseStatus.RunSpeed - 1f;
        }
        else
        {
            SetAnimation(AnimHash.RunHash_1);
            _NavMeshAgent.speed = entity.baseStatus.RunSpeed;
        }
        _NavMeshAgent.SetDestination(Player.Instance.transform.position);
    }
    public override void Update()
    {
        if (true == NaviHelper.IsReached(_NavMeshAgent, 1f))
        {
            StateMachine.SetState(EntityEnum.Attack);
        }
    }
    public override void Exit()
    {
    }
}

public class AttackState : EntityState
{
    public override void SetOwner(Entity _Entity, EntityStateMachine _StateMachine)
    {
        _EntityEnum = EntityEnum.Attack;
        base.SetOwner(_Entity, _StateMachine);
    }

    public override void Enter()
    {
        if (Vector3.Distance(entity.transform.position, Player.Instance.transform.position) > 10)
        {
            StateMachine.SetState(EntityEnum.Idle);
            return;
        }
        if (Random.Range(0, 2) == 0)
        {
            SetAnimation(AnimHash.AttackHash_1);
        }
        else
        {
            SetAnimation(AnimHash.AttackHash_2);
        }
        entity._NavMeshAgent.ResetPath();
    }
    public override void Update()
    {
        if (true == IsAnimationEnd())
        {
            StateMachine.SetState(EntityEnum.Run);
        }
    }
    public override void Exit()
    {
    }
}

public class HitState : EntityState
{
    public override void SetOwner(Entity _Entity, EntityStateMachine _StateMachine)
    {
        _EntityEnum = EntityEnum.Hit;
        base.SetOwner(_Entity, _StateMachine);
    }

    public override void Enter()
    {
        entity._NavMeshAgent.ResetPath();
        if (Random.Range(0, 2) == 0)
        {
            SetAnimation(AnimHash.HitHash_1);
        }
        else
        {
            SetAnimation(AnimHash.HitHash_2);
        }
    }
    public override void Update()
    {
        if (true == IsAnimationEnd())
        {
            StateMachine.SetState(EntityEnum.Idle);
        }
    }
    public override void Exit()
    {
    }
}

public class HurtState : EntityState
{
    public override void SetOwner(Entity _Entity, EntityStateMachine _StateMachine)
    {
        _EntityEnum = EntityEnum.Hurt;
        base.SetOwner(_Entity, _StateMachine);
    }

    public override void Enter()
    {
        entity._NavMeshAgent.ResetPath();
        SetAnimation(AnimHash.DieHash);
    }
    public override void Update()
    {
        if (true == IsAnimationEnd())
        {
            StateMachine.SetState(EntityEnum.Dying);
        }
    }
    public override void Exit()
    {
    }
}

public class DyingState : EntityState
{
    public override void SetOwner(Entity _Entity, EntityStateMachine _StateMachine)
    {
        _EntityEnum = EntityEnum.Dying;
        base.SetOwner(_Entity, _StateMachine);
    }

    public override void Enter()
    {
        entity._NavMeshAgent.SetDestination(Player.Instance.transform.position);
        entity._NavMeshAgent.speed = entity.baseStatus.CrowlSpeed;
        SetAnimation(AnimHash.HurtHash);
    }
    public override void Update()
    {
        if (true == IsAnimationEnd())
        {
            entity.Dead();
        }
    }
    public override void Exit()
    {
    }
}

public class DieState : EntityState
{
    public override void SetOwner(Entity _Entity, EntityStateMachine _StateMachine)
    {
        _EntityEnum = EntityEnum.Die;
        base.SetOwner(_Entity, _StateMachine);
    }

    public override void Enter()
    {
        entity.Dead();
    }
    public override void Update()
    {
    }
    public override void Exit()
    {
    }
}

