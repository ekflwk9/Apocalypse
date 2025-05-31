using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeZombieStruct : StateStruct
{
    public override void Init(EntityStateMachine _StateMachine)
    {
        IdleState idleState = new IdleState();
        WalkState walkState = new WalkState();
        HearingState hearingState = new HearingState();
        RangeRunState RunState = new RangeRunState();
        RangeAttackState attackState = new RangeAttackState();
        HitState hitState = new HitState();
        DyingState dyingState = new DyingState();
        HurtState hurtState = new HurtState();
        DieState dieState = new DieState();

        StateDictionary.Add(EntityEnum.Idle, idleState);
        StateDictionary.Add(EntityEnum.Walk, walkState);
        StateDictionary.Add(EntityEnum.Hearing, hearingState);
        StateDictionary.Add(EntityEnum.Attack, attackState);
        StateDictionary.Add(EntityEnum.Hit, hitState);
        StateDictionary.Add(EntityEnum.Run, RunState);
        StateDictionary.Add(EntityEnum.Dying, dyingState);
        StateDictionary.Add(EntityEnum.Hurt, hurtState);
        StateDictionary.Add(EntityEnum.Die, dieState);
        base.Init(_StateMachine);
    }
}


public class RangeZombie : Entity
{
    [SerializeField] GameObject Projectile;

    private void Awake()
    {
        _stateMachine = new EntityStateMachine();
        _stateMachine.Init(new RangeZombieStruct(), this);

        baseStatus = new BaseStatus();

        _stateMachine.SetState(EntityEnum.Idle);

        baseStatus.SetStatus(20, 10);

    }

    public override void Attack_2()
    {
        Vector3 position = transform.position + new Vector3(0, 1, 0);
        ObjectPool.Instance.Get(Projectile).transform.position = position;
    }
}
