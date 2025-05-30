using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivorRunState : EntityState
{
    float CurrentTime = 0.5f;
    float ResetTime = 0f;

    public override void SetOwner(Entity _Entity, EntityStateMachine _StateMachine)
    {
        _EntityEnum = EntityEnum.Run;
        base.SetOwner(_Entity, _StateMachine);
    }

    public override void Enter()
    {
        SetAnimationForce(AnimHash.RunHash_1);
        entity._NavMeshAgent.speed = entity.baseStatus.RunSpeed;
        CurrentTime = 0.5f;
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

            entity._NavMeshAgent.SetDestination(playerPos);
            CurrentTime = 0f;
        }

        if (true == entity._NavMeshAgent.pathPending)
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
        else if (Distance < entity.baseStatus.DetectedRange)
        {
            if (0 > Vector3.Dot(Player.Instance.transform.forward, entity.transform.forward))
            {
                StateMachine.SetState(EntityEnum.Detect);
            }
        }
    }

    public override void Exit()
    {
    }
}


public class SurvivorDetectedState : EntityState
{
    Vector3 HideLocation;
    bool CanHide = false;
    float CurrentTime = 0.5f;
    float ResetTime = 0f;

    public override void SetOwner(Entity _Entity, EntityStateMachine _StateMachine)
    {
        _EntityEnum = EntityEnum.Detect;
        base.SetOwner(_Entity, _StateMachine);
    }

    public override void Enter()
    {
        SetAnimationForce(AnimHash.SneakMoveHash);
        CanHide = Detect();

        if (CanHide == false)
        {
            SetAnimationForce(AnimHash.RunHash_1);
            entity._NavMeshAgent.speed = entity.baseStatus.RunSpeed;
            return;
        }


        entity._NavMeshAgent.SetDestination(HideLocation);
    }
    public override void Update()
    {
        if (CanHide == false)
        {
            ForceRun();
            return;
        }

        Vector3 playerPos = Player.Instance.transform.position;
        Vector3 entityPos = entity.transform.position;

        float Distance = Vector3.Distance(playerPos, entityPos);

        if (Distance < 9)
        {
            StateMachine.SetState(EntityEnum.Run);
            return;
        }

        if (1f > entity._NavMeshAgent.remainingDistance)
        {
            SetAnimation(AnimHash.SneakHash);
            if (Distance < entity.baseStatus.DetectedRange)
            {
                if (0 > Vector3.Dot(Player.Instance.transform.forward, entity.transform.forward))
                {
                    return;
                }
            }

            if (0 < Vector3.Dot(Player.Instance.transform.forward, entity.transform.forward))
            {
                if (Distance < entity.baseStatus.DetectedRange / 2)
                {
                    StateMachine.SetState(EntityEnum.Run);
                    return;
                }
                else
                {
                    StateMachine.SetState(EntityEnum.Detect);
                    return;
                }
            }
            else if (Distance > entity.baseStatus.DetectedRange)
            {
                StateMachine.SetState(EntityEnum.Run);
                return;
            }
        }
    }


    public override void Exit()
    {
    }

    List<Obstacle> obstacles = new List<Obstacle>();

    public bool Detect()
    {
        HideLocation = Vector3.zero;
        //오버랩 된넘들
        obstacles.Clear();
        int Layer = LayerHelper.GetLayer(LayerHelper.Obstacle);
        Collider[] targets = Physics.OverlapSphere(entity.transform.position, entity.baseStatus.DetectedRange, Layer);

        //range for
        foreach (var target in targets)
        {
            ObstacleContainer obstacleContainer = target.GetComponent<ObstacleContainer>();

            foreach (Obstacle obstacle in obstacleContainer.GetObstacles())
            {
                if (obstacle != null && obstacle.CheckHide() == true)
                {
                    obstacles.Add(obstacle);
                }
            }
        }

        if (obstacles.Count == 0)
        {
            Vector3 TargetPos = NaviHelper.GetRandomNavMeshPosition(entity.transform.position, 30f);
            HideLocation = TargetPos;
            return false;
        }


        Vector3 playerPos = Player.Instance.transform.position;

        Vector3 PlayerDirection = (playerPos - entity.transform.position).normalized;

        float Distance = 0;
        float CurrentDistance = 0;
        foreach (Obstacle obstacle in obstacles)
        {
            Distance = Vector3.Distance(entity.transform.position, obstacle.transform.position);
            Vector3 HideDirection = (obstacle.transform.position - entity.transform.position).normalized;

            if (HideLocation == Vector3.zero)
            {
                if (0.5 < Vector3.Dot(HideDirection, PlayerDirection))
                {
                    HideLocation = obstacle.transform.position;
                    CurrentDistance = Distance;
                }
            }
            else
            {
                if (CurrentDistance > Distance)
                {
                    if (0 < Vector3.Dot(HideDirection, PlayerDirection))
                    {
                        HideLocation = obstacle.transform.position;
                        CurrentDistance = Distance;   // 작으면 더 작은걸로 바뀜
                    }
                }
            }
        }
        if (HideLocation == Vector3.zero)
        {
            return false;
        }
        return true;
    }

    void ForceRun()
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

            entity._NavMeshAgent.SetDestination(playerPos);
            CurrentTime = 0f;
        }

        if (true == entity._NavMeshAgent.pathPending)
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
}



public class SurvivorWalkState : EntityState
{
    Vector3 HideLocation;
    List<Obstacle> obstacles = new List<Obstacle>();
    Coroutine waitCoroutine;

    public override void SetOwner(Entity _Entity, EntityStateMachine _StateMachine)
    {
        _EntityEnum = EntityEnum.Walk;
        base.SetOwner(_Entity, _StateMachine);
    }

    public override void Enter()
    {
        Detect();
        entity._NavMeshAgent.SetDestination(HideLocation);
        SetAnimation(AnimHash.WalkHash);
    }
    public override void Update()
    {
        if (1f > entity._NavMeshAgent.remainingDistance)
        {
            waitCoroutine = CoroutineManager.Instance.SetCoroutine(entity, WaitCoroutine());
        }
    }
    public override void Exit()
    {
        CoroutineManager.Instance.UnSetCoroutine(entity, waitCoroutine);
    }

    IEnumerator WaitCoroutine()
    {
        SetAnimation(AnimHash.IdleHash);
        entity._NavMeshAgent.ResetPath();
        yield return CoroutineHelper.GetTime(5f);
        StateMachine.SetState(EntityEnum.Idle);
    }

    public void Detect()
    {
        HideLocation = new Vector3(0, 0, 0);
        //오버랩 된넘들
        obstacles.Clear();
        int Layer = LayerHelper.GetLayer(LayerHelper.Obstacle);
        Collider[] targets = Physics.OverlapSphere(entity.transform.position, entity.baseStatus.DetectedRange, Layer);

        //range for
        foreach (var target in targets)
        {
            ObstacleContainer obstacleContainer = target.GetComponent<ObstacleContainer>();

            foreach (Obstacle obstacle in obstacleContainer.GetObstacles())
            {
                if (obstacle != null && obstacle.CheckHide() == false)
                {
                    obstacles.Add(obstacle);
                }
            }
        }

        if (obstacles.Count == 0)
        {
            Vector3 TargetPos = NaviHelper.GetRandomNavMeshPosition(entity.transform.position, 30f);
            HideLocation = TargetPos;
            return;
        }

        int obstacleCount = obstacles.Count;
        HideLocation = obstacles[Random.Range(0, obstacleCount)].transform.position;

    }
}
