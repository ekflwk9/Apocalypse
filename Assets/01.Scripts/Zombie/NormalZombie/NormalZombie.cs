using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalZombieStateStruct : StateStruct
{
    public override void Init(EntityStateMachine _StateMachine)
    {
        IdleState idleState = new IdleState();
        WalkState walkState = new WalkState();
        HearingState hearingState = new HearingState();
        RunState runState = new RunState();
        AttackState attackState = new AttackState();
        HitState hitState = new HitState();
        HurtState hurtState = new HurtState();
        DyingState dyingState = new DyingState();
        DieState dieState = new DieState();
        StateDictionary.Add(EntityEnum.Idle, idleState);
        StateDictionary.Add(EntityEnum.Hearing, hearingState);
        StateDictionary.Add(EntityEnum.Run, runState);
        StateDictionary.Add(EntityEnum.Attack, attackState);
        StateDictionary.Add(EntityEnum.Walk, walkState);
        StateDictionary.Add(EntityEnum.Hit, hitState);
        StateDictionary.Add(EntityEnum.Hurt, hurtState);
        StateDictionary.Add(EntityEnum.Dying, dyingState);
        StateDictionary.Add(EntityEnum.Die, dieState);
        base.Init(_StateMachine);
    }
}


public class NormalZombie : Entity
{
    private void Awake()
    {
        _stateMachine = new EntityStateMachine();
        _stateMachine.Init(new NormalZombieStateStruct(), this);

        baseStatus = new BaseStatus();

        _stateMachine.SetState(EntityEnum.Idle);
    }
}
