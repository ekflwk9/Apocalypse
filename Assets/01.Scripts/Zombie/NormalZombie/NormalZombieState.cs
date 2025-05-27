using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.VersionControl.Asset;

public static class AnimHash
{
    public static readonly int IdleHash = Animator.StringToHash("Idle");
    public static readonly int WalkHash = Animator.StringToHash("Walking");
    public static readonly int RunHash = Animator.StringToHash("Run_1");
    public static readonly int HitHash = Animator.StringToHash("Hit_1");
    public static readonly int AttackHash = Animator.StringToHash("Attack_1");
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
        StateDictionary.Add(EntityEnum.Idle, idleState);
        StateDictionary.Add(EntityEnum.Walk, walkState);
        StateDictionary.Add(EntityEnum.Run, runState);
        StateDictionary.Add(EntityEnum.Attack, attackState);
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
        SetAnimation(AnimHash.RunHash);
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

        SetAnimation(AnimHash.AttackHash);
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
        SetAnimation(AnimHash.HitHash);
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

