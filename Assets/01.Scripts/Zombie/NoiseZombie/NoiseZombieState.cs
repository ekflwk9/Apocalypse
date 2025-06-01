using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NoiseAttackState : EntityState
{
    public override void SetOwner(Entity _Entity, EntityStateMachine _StateMachine)
    {
        _EntityEnum = EntityEnum.Attack;
        base.SetOwner(_Entity, _StateMachine);
    }

    public override void Enter()
    {
        SetDirection();

        Vector3 entityPos = entity.transform.position;
        Vector3 playerPos = Player.Instance.transform.position;
        float Distance = Vector3.Distance(playerPos, entityPos);
        if (Distance < 1f)
        {
            int value = Random.Range(0, 2);
            if (value == 0)
            {
                SetAnimationForce(AnimHash.AttackHash_1);
            }
            else
            {
                SetAnimationForce(AnimHash.AttackHash_2);
            }
        }
        else 
        {
            SetAnimationForce(AnimHash.YellingHash);
        }
        entity._NavMeshAgent.ResetPath();
    }
    public override void Update()
    {
        if (true == IsAnimationEnd(1))
        {
            float Distance = Vector3.Distance(Player.Instance.transform.position, entity.transform.position);
            if (Distance < 10)
            {
                StateMachine.SetState(EntityEnum.Attack);
            }
            else
            {
                StateMachine.SetState(EntityEnum.Idle);
            }
        }
    }
    public override void Exit()
    {
    }
}

public class NoiseHitState : EntityState
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
            SetAnimationForce(AnimHash.HitHash_1);
        }
        else
        {
            SetAnimationForce(AnimHash.HitHash_2);
        }
    }
    public override void Update()
    {
        if (true == IsAnimationEnd())
        {
            Vector3 playerPos = Player.Instance.transform.position;
            Vector3 entityPos = entity.transform.position;
            float Distance = Vector3.Distance(entityPos, playerPos);
            if (Distance > 10)
            {
                StateMachine.SetState(EntityEnum.Attack);
            }
            else
            {
                StateMachine.SetState(EntityEnum.Idle);
            }
        }
    }
    public override void Exit()
    {
    }
}