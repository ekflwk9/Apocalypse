using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;



public class Entity : MonoBehaviour, IDamagable
{
    [SerializeField] protected Rigidbody _rigidbody;
    [SerializeField] protected CapsuleCollider _capsuleCollider;
    [SerializeField] protected EntityAttack _entityAttack;

    protected EntityStateMachine _stateMachine;
    public BaseStatus baseStatus;
    public NavMeshAgent _NavMeshAgent;
    public Animator _animator;
    [SerializeField] LayerMask PlayerMask;

    [SerializeField] Collider[] ragdollColliders;
    [SerializeField] Rigidbody[] ragdollRigidbodies;


    protected void Reset()
    {
        _rigidbody = GetComponent<Rigidbody>();
        if (_rigidbody == null)
        {
            _rigidbody = gameObject.AddComponent<Rigidbody>();
            _rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }

        _NavMeshAgent = GetComponent<NavMeshAgent>();

        if (_NavMeshAgent == null)
        {
            _NavMeshAgent = gameObject.AddComponent<NavMeshAgent>();
            _NavMeshAgent.updateRotation = false;
            _NavMeshAgent.updateUpAxis = false;
        }

        _capsuleCollider = GetComponent<CapsuleCollider>();
        if (_capsuleCollider == null)
        {
            _capsuleCollider = gameObject.AddComponent<CapsuleCollider>();
            _capsuleCollider.height = 2f;
            _capsuleCollider.radius = 0.5f;
            _capsuleCollider.center = Vector3.up;
        }

        _entityAttack = GetComponentInChildren<EntityAttack>();

        if (_entityAttack == null)
        {
            _entityAttack = gameObject.AddComponent<EntityAttack>();
        }


        _animator = GetComponentInChildren<Animator>();

        PlayerMask = LayerMask.GetMask("Player");

        // linq고 gpt꺼 따옴
        ragdollColliders = GetComponentsInChildren<Collider>(true)
    .Where(c => c.gameObject != this.gameObject)
    .ToArray();

        ragdollRigidbodies = GetComponentsInChildren<Rigidbody>(true)
            .Where(rb => rb.gameObject != this.gameObject)
            .ToArray();


        foreach (var col in ragdollColliders)
        {
            col.enabled = false; // 초기에는 비활성화
        }

        foreach (var rb in ragdollRigidbodies)
        {
            rb.isKinematic = true; // 초기에는 Kinematic 설정
        }

    }

    private void OnEnable()
    {
        SetRagdollActive(false);
    }

    protected void Update()
    {
        _stateMachine.Update();
        Detect();
    }

    void Detect()
    {
        if (_stateMachine.GetState() != EntityEnum.Idle)
        {
            return;
        }

        //오버랩 된넘들
        Collider[] targets = Physics.OverlapSphere(transform.position, 20f, PlayerMask);

        //range for
        foreach (var target in targets)
        {
            Vector3 dirToTarget = (target.transform.position - transform.position).normalized;
            float angle = Vector3.Angle(transform.forward, dirToTarget);

            //왼쪽 45도고 오른쪽 45도니 총 90도임
            if (angle < 45f)
            {
                if (Physics.Raycast(transform.position + Vector3.up * 0.5f, dirToTarget, out RaycastHit hit, 20f))
                {
                    //임시
                    if (hit.collider.CompareTag("Player"))
                    {
                        _stateMachine.SetState(EntityEnum.Run);
                        Debug.Log("확인");
                    }
                }
            }
        }
    }

    public void Dead()
    {
        _entityAttack.StopAttack();
        SetRagdollActive(true);
    }

    private void OnDrawGizmos()
    {
        // 감지 반경 색상
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 20);

        // 시야각 (viewAngle)을 시각화 (좌우)
        Vector3 leftDir = Quaternion.Euler(0, -90f / 2f, 0) * transform.forward;
        Vector3 rightDir = Quaternion.Euler(0, 90f / 2f, 0) * transform.forward;

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, leftDir * 20);
        Gizmos.DrawRay(transform.position, rightDir * 20);

        // (선택) 정면 방향 표시
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, transform.forward * 20);
    }

    public void Attack()
    {
        _entityAttack.Attack();
    }


    public void TakeDamage(float damage)
    {
        if (_stateMachine.GetState() == EntityEnum.Die)
        {
            return;
        }

        if (_stateMachine.GetState() == EntityEnum.Hurt || _stateMachine.GetState() == EntityEnum.Dying)
        {
            _stateMachine.SetState(EntityEnum.Die);
            return;
        }

        baseStatus.CurrentHp -= damage;

        if (baseStatus.CurrentHp <= 0)
        {
            _stateMachine.SetState(EntityEnum.Die);
            return;
        }


        _stateMachine.SetState(EntityEnum.Hit);
    }

    public void SetRagdollActive(bool isActive)  //true면 ragdoll 활성화, false면 비활성화
    {
        foreach (var col in ragdollColliders)
        {
            col.enabled = isActive;
        }

        // 리지드바디 kinematic 설정 (false = 물리 적용, true = 비활성화)
        foreach (var rb in ragdollRigidbodies)
        {
            rb.isKinematic = !isActive;
        }

        // 메인 콜라이더/리지드바디 끄거나 켜기
        _capsuleCollider.enabled = !isActive;
        _rigidbody.isKinematic = isActive;

        // 네브메시나 애니메이터도 끄는게 일반적입니다
        _NavMeshAgent.enabled = !isActive;
        _animator.enabled = !isActive;
    }



}
