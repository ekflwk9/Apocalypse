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
    public override void Init()
    {
        IdleState idleState = new IdleState();
        RunState runState = new RunState();
        StateDictionary.Add(EntityEnum.Idle, idleState);
        StateDictionary.Add(EntityEnum.Run, runState);
        base.Init();
    }
}

public class IdleState : EntityState
{
    public override void Enter()
    {
        entity._animator.Play(AnimHash.IdleHash);
    }
    public override void Update()
    {
        // Logic for idle state
    }
    public override void Exit()
    {
    }
}

public class WalkState : EntityState
{
    NavMeshAgent _NavMeshAgent;
    public override void Enter()
    {
        if (true != IsInit)
        {
            _NavMeshAgent = entity.GetComponent<NavMeshAgent>();
            IsInit = true;
        }
        entity._animator.Play(AnimHash.WalkHash);
    }
    public override void Update()
    {
    }
    public override void Exit()
    {
    }
}

public class RunState : EntityState
{
    NavMeshAgent _NavMeshAgent;
    public override void Enter()
    {
        if (true != IsInit)
        {
            _NavMeshAgent = entity.GetComponent<NavMeshAgent>();
            IsInit = true;
        }
        entity._animator.Play(AnimHash.RunHash);
    }
    public override void Update()
    {
    }
    public override void Exit()
    {
    }
}
