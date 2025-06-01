using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
        entity._NavMeshAgent.speed = entity.baseStatus.WalkSpeed;
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
        entity._NavMeshAgent.speed = entity.baseStatus.RunSpeed;

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

        float HideDistance = Vector3.Distance(HideLocation, entityPos);

        Vector3 Direction = (playerPos - entityPos).normalized;

        if (Distance < 9)
        {
            StateMachine.SetState(EntityEnum.Run);
            return;
        }

        if (1.5f > HideDistance)
        {
            SetAnimation(AnimHash.SneakHash);
            entity._NavMeshAgent.ResetPath();
            if (Distance < entity.baseStatus.DetectedRange)
            {
                if (0 > Vector3.Dot(Player.Instance.transform.forward, Direction))
                {
                    return;
                }
            }

            if (0 < Vector3.Dot(Player.Instance.transform.forward, Direction))
            {
                StateMachine.SetState(EntityEnum.Run);
                //if (Distance < entity.baseStatus.DetectedRange / 2)
                //{
                //    return;
                //}
                //else
                //{
                //    StateMachine.SetState(EntityEnum.Detect);
                //    return;
                //}
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

        float PlayerDistance = Vector3.Distance(playerPos, entity.transform.position);

        float Distance = 0;
        float CurrentDistance = 0;
        foreach (Obstacle obstacle in obstacles)
        {
            Distance = Vector3.Distance(entity.transform.position, obstacle.transform.position);

            if (PlayerDistance < Distance)
            {
                continue;
            }

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
                SetUpperAnimation(AnimHash.AttackHash_1);
                SetBottomAnimation(AnimHash.RunHash_1);
            }
        }
    }
}

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
            return;
        }

        else if (Distance < entity.baseStatus.AttackRange + 4f)
        {
            if (IsAnimationEnd(1) == true)
            {
                SetUpperAnimationForce(AnimHash.AttackHash_1);
            }
            SetBottomAnimation(AnimHash.RunHash_1);
            return;
        }

        else if (Distance < entity.baseStatus.DetectedRange / 3)
        {
            return;
        }
        if (0 > Vector3.Dot(Player.Instance.transform.forward, directionPlayer))
        {
            StateMachine.SetState(EntityEnum.Detect);
        }
    }

    public override void Exit()
    {
    }
}

public class SurvivorAttackState : EntityState
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
        //if (IsAnimationEnd(1) == true)
        //{
        //    SetDirection();

        //    if (Random.Range(0, 2) == 0)
        //    {
        //        SetAnimation(AnimHash.AttackHash_1);
        //    }
        //    else
        //    {
        //        SetAnimation(AnimHash.AttackHash_2);
        //    }
        //}
        if (IsAnimationEnd(1) == true)
        {
            SetAnimation(AnimHash.AttackHash_1);
        }
        else
        {
            SetBottomAnimation(AnimHash.IdleHash);
        }
        SetDirection();
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
                SetAnimationForce(AnimHash.AttackHash_1);
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