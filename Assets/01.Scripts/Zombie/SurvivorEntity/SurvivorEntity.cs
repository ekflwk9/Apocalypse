using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivorEntityStruct : StateStruct
{
    public override void Init(EntityStateMachine _StateMachine)
    {
        IdleState idleState = new IdleState();
        SurvivorWalkState walkState = new SurvivorWalkState();
        SurvivorDetectedState detectState = new SurvivorDetectedState();
        SurvivorRunState runState = new SurvivorRunState();
        SurvivorAttackState attackState = new SurvivorAttackState();
        HitState hitState = new HitState();
        HurtState hurtState = new HurtState();
        DyingState dyingState = new DyingState();
        DieState dieState = new DieState();



        StateDictionary.Add(EntityEnum.Idle, idleState);
        StateDictionary.Add(EntityEnum.Walk, walkState);
        StateDictionary.Add(EntityEnum.Run, runState);
        StateDictionary.Add(EntityEnum.Detect, detectState);
        StateDictionary.Add(EntityEnum.Attack, attackState);
        StateDictionary.Add(EntityEnum.Hit, hitState);
        StateDictionary.Add(EntityEnum.Hurt, hurtState);
        StateDictionary.Add(EntityEnum.Dying, dyingState);
        StateDictionary.Add(EntityEnum.Die, dieState);

        base.Init(_StateMachine);

    }
}


public class SurvivorEntity : Entity
{
    private void Awake()
    {
        _stateMachine = new EntityStateMachine();
        _stateMachine.Init(new SurvivorEntityStruct(), this);

        baseStatus = new BaseStatus();

        _stateMachine.SetState(EntityEnum.Idle);

        baseStatus.SetStatus(30, 2, 90);

    }

    protected override void Update()
    {
        _stateMachine.Update();
        if (_stateMachine.GetState() == EntityEnum.Idle || _stateMachine.GetState() == EntityEnum.Walk)
        {
            Detect();
        }
    }

    protected override void Detect()
    {
        //오버랩 된넘들
        Collider[] targets = Physics.OverlapSphere(transform.position, baseStatus.DetectedRange, PlayerMask);

        //range for
        foreach (var target in targets)
        {
            Vector3 dirToTarget = (target.transform.position - transform.position).normalized;
            float angle = Vector3.Angle(transform.forward, dirToTarget);

            //왼쪽 45도고 오른쪽 45도니 총 90도임
            if (angle < baseStatus.DetectedAngle)
            {
                if (Physics.Raycast(transform.position + Vector3.up * 0.5f, dirToTarget, out RaycastHit hit, baseStatus.DetectedRange))
                {
                    if (hit.collider.CompareTag(TagHelper.Player))
                    {
                        _stateMachine.SetState(EntityEnum.Run);
                    }
                }
            }
        }
    }

    public override void Dead()
    {
        gameObject.PlayAudio("SurvivorDead");
        SetRagdollActive(true);
        CoroutineManager.Instance.UnSetAllCoroutine(this);
        StartCoroutine(OnRelease());
    }

    IEnumerator OnRelease()
    {
        yield return CoroutineHelper.GetTime(5f);

        ObjectPool.Instance.Set(ContentManager.GetAsset<GameObject>(PrefabName), gameObject);
    }

    //public override void Attack_2()
    //{
    //    Vector3 position = transform.position + new Vector3(0, 1, 0);
    //    ObjectPool.Instance.Get(Projectile).transform.position = position;
    //}
}
