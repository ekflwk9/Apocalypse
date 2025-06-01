using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class NoiseZombieStruct : StateStruct
{
    public override void Init(EntityStateMachine _StateMachine)
    {
        IdleState idleState = new IdleState();
        WalkState walkState = new WalkState();
        HearingState hearingState = new HearingState();
        RunState runState = new RunState();
        NoiseAttackState attackState = new NoiseAttackState();
        NoiseHitState hitState = new NoiseHitState();
        HurtState hurtState = new HurtState();
        DyingState dyingState = new DyingState();
        DieState dieState = new DieState();
        StateDictionary.Add(EntityEnum.Idle, idleState);
        StateDictionary.Add(EntityEnum.Attack, attackState);
        StateDictionary.Add(EntityEnum.Walk, walkState);
        StateDictionary.Add(EntityEnum.Hit, hitState);
        StateDictionary.Add(EntityEnum.Hurt, hurtState);
        StateDictionary.Add(EntityEnum.Dying, dyingState);
        StateDictionary.Add(EntityEnum.Die, dieState);
        base.Init(_StateMachine);
    }
}



public class NoiseZombie : Entity
{
    [SerializeField] ZombieHearingComponent _zombieHearingComponent;

    protected override void Reset()
    {
        base.Reset();
        _zombieHearingComponent = GetComponentInChildren<ZombieHearingComponent>();
    }

    private void Awake()
    {
        _stateMachine = new EntityStateMachine();
        _stateMachine.Init(new NoiseZombieStruct(), this);

        baseStatus = new BaseStatus();

        _stateMachine.SetState(EntityEnum.Idle);


        baseStatus.SetStatus(20, 10);
    }

    public override void Attack_2()
    {
        _zombieHearingComponent.OnSound(baseStatus.DetectedRange, Player.Instance.transform.position);
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
                    //임시
                    if (hit.collider.CompareTag(TagHelper.Player))
                    {
                        baseStatus.DetectedLocation = hit.point; // 플레이어 위치 저장
                        _stateMachine.SetState(EntityEnum.Attack);
                    }
                }
            }
        }
    }

}


