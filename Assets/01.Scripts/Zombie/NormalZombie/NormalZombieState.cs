using System.Collections;
using UnityEngine;
using UnityEngine.AI;

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
        entity.baseStatus.DetectedLocation = TargetPos;
        _NavMeshAgent.SetDestination(TargetPos);
        _NavMeshAgent.speed = entity.baseStatus.WalkSpeed;
        SetAnimation(AnimHash.WalkHash);
    }
    public override void Update()
    {
        float Distance = Vector3.Distance(entity.transform.position, entity.baseStatus.DetectedLocation);
        if (Distance < 3f)
        {
            StateMachine.SetState(EntityEnum.Idle);
            entity._NavMeshAgent.ResetPath();
        }
    }
    public override void Exit()
    {
    }
}


public class HearingState : EntityState
{
    NavMeshAgent _NavMeshAgent;

    public override void SetOwner(Entity _Entity, EntityStateMachine _StateMachine)
    {
        _EntityEnum = EntityEnum.Hearing;
        base.SetOwner(_Entity, _StateMachine);
    }

    public override void Enter()
    {
        Vector3 detectedPos = entity.baseStatus.DetectedLocation;
        Vector3 entityPos = entity.transform.position;

        if (true != IsInit)
        {
            _NavMeshAgent = entity.GetComponent<NavMeshAgent>();
            IsInit = true;
        }

        if (entity.baseStatus.IsHalf() == true)
        {
            SetAnimation(AnimHash.RunHash_2);
            _NavMeshAgent.speed = entity.baseStatus.RunSpeed / 2f;
        }
        else
        {
            SetAnimation(AnimHash.RunHash_1);
            _NavMeshAgent.speed = entity.baseStatus.RunSpeed;
        }
        _NavMeshAgent.ResetPath();
        _NavMeshAgent.SetDestination(detectedPos);
    }

    public override void Update()
    {
        Vector3 detectedPos = entity.baseStatus.DetectedLocation;
        Vector3 entityPos = entity.transform.position;

        float Distance = Vector3.Distance(detectedPos, entityPos);


        if ((Distance < 3))
        {
            StateMachine.SetState(EntityEnum.Idle);
            entity._NavMeshAgent.ResetPath();
        }
    }

    public override void Exit()
    {
    }
}

public class RunState : EntityState
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

        else if (Distance < entity.baseStatus.AttackRange + 4f)
        {
            if (IsAnimationEnd(1) == true)
            {
                if (Random.Range(0, 2) == 0)
                {
                    SetUpperAnimation(AnimHash.AttackHash_1);
                }
                else
                {
                    SetUpperAnimation(AnimHash.AttackHash_2);
                }
            }
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
        SetDirection();
        if (Vector3.Distance(entity.transform.position, Player.Instance.transform.position) > 10)
        {
            StateMachine.SetState(EntityEnum.Idle);
            return;
        }
        if (IsAnimationEnd(1) == true)
        {
            if (Random.Range(0, 2) == 0)
            {
                SetAnimation(AnimHash.AttackHash_1);
            }
            else
            {
                SetAnimation(AnimHash.AttackHash_2);
            }
        }
        else
        {
            SetBottomAnimation(AnimHash.IdleHash);
        }
        entity._NavMeshAgent.ResetPath();
    }
    public override void Update()
    {
        if (true == IsAnimationEnd(1))
        {
            Vector3 playerPos = Player.Instance.transform.position;
            Vector3 entityPos = entity.transform.position;
            Vector3 directionPlayer = (playerPos - entityPos).normalized;
            Quaternion LookDirection = Quaternion.LookRotation(directionPlayer);
            entity.transform.rotation = LookDirection;

            float Distance = Vector3.Distance(entityPos, playerPos);
            if (Distance < entity.baseStatus.AttackRange)
            {
                if (Random.Range(0, 2) == 0)
                {
                    SetAnimation(AnimHash.AttackHash_1);
                }
                else
                {
                    SetAnimation(AnimHash.AttackHash_2);
                }
            }
            else if (Distance < 10f)
            {
                StateMachine.SetState(EntityEnum.Run);
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
            if (Distance < entity.baseStatus.AttackRange)
            {
                StateMachine.SetState(EntityEnum.Attack);
            }
            else if (Distance < 10f)
            {
                StateMachine.SetState(EntityEnum.Run);
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

//쓰러지기 시작   이 때 맞으면 디져야함
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
        if (true == IsAnimationEnd())   // 다 쓰러지면 기어가다 죽음
        {
            StateMachine.SetState(EntityEnum.Dying);
        }
    }
    public override void Exit()
    {
    }
}

//이제 기어감 이때 맞아도 디져야함
public class DyingState : EntityState
{
    public override void SetOwner(Entity _Entity, EntityStateMachine _StateMachine)
    {
        _EntityEnum = EntityEnum.Dying;
        base.SetOwner(_Entity, _StateMachine);
    }

    public override void Enter()
    {
        SetDirection();
        entity._NavMeshAgent.SetDestination(Player.Instance.transform.position);
        entity._NavMeshAgent.speed = entity.baseStatus.CrowlSpeed;
        SetAnimationFade(AnimHash.HurtHash, 0.2f);
        CoroutineManager.Instance.SetCoroutine(entity, DieCoroutine());
    }
    public override void Update()
    {

    }
    public override void Exit()
    {
    }

    IEnumerator DieCoroutine()
    {
        yield return CoroutineHelper.GetTime(5.0f);
        entity.Dead();
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

