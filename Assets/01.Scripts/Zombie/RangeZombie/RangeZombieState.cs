using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class RangeAttackState : EntityState
{
    public override void SetOwner(Entity _Entity, EntityStateMachine _StateMachine)
    {
        _EntityEnum = EntityEnum.Attack;
        base.SetOwner(_Entity, _StateMachine);
    }

    public override void Enter()
    {
        entity._NavMeshAgent.ResetPath();
        SetDirection();
        Vector3 entityPos = entity.transform.position;
        Vector3 playerPos = Player.Instance.transform.position;
        float Distance = Vector3.Distance(playerPos, entityPos);
        if (Distance < 2f)
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
        else if (Distance < entity.baseStatus.AttackRange)
        {
            SetAnimationForce(AnimHash.YellingHash);
        }

    }
    public override void Update()
    {
        if (true == IsAnimationEnd(1))
        {
            float Distance = Vector3.Distance(Player.Instance.transform.position, entity.transform.position);
            if (Distance < entity.baseStatus.AttackRange)
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


public class RangeRunState : EntityState
{
    NavMeshAgent _NavMeshAgent;
    float ResetTime = 0.5f;
    float CurrentTime = 0f;

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
            SetAnimationForce(AnimHash.RunHash_2);
            _NavMeshAgent.speed = entity.baseStatus.RunSpeed / 2f;
        }
        else
        {
            SetAnimationForce(AnimHash.RunHash_1);
            _NavMeshAgent.speed = entity.baseStatus.RunSpeed;
        }
        CurrentTime = ResetTime;
        _NavMeshAgent.ResetPath();
    }

    public override void Update()
    {
        CurrentTime += Time.deltaTime;
        Vector3 playerPos = Player.Instance.transform.position;
        Vector3 entityPos = entity.transform.position;

        Vector3 directionPlayer = (playerPos - entityPos).normalized;
        Quaternion LookDirection = Quaternion.LookRotation(directionPlayer);
        entity.transform.rotation = LookDirection;


        float Distance = Vector3.Distance(entityPos, playerPos);

        if (CurrentTime >= ResetTime)
        {

            _NavMeshAgent.SetDestination(playerPos);
            CurrentTime = 0f;
        }

        if (true == _NavMeshAgent.pathPending)
        {
            return;
        }

        if (Distance < entity.baseStatus.AttackRange)
        {
            StateMachine.SetState(EntityEnum.Attack);
        }

    }

    public override void Exit()
    {
    }
}