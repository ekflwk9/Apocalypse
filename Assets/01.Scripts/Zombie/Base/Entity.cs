using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



public class Entity : MonoBehaviour, IDamagable
{
    [SerializeField] protected Rigidbody _rigidbody;
    [SerializeField] protected CapsuleCollider _capsuleCollider;
    protected EntityStateMachine _stateMachine;
    public BaseStatus baseStatus;
    public NavMeshAgent _NavMeshAgent;
    public Animator _animator;
    [SerializeField] LayerMask PlayerMask;


    protected void Reset()
    {
        _rigidbody = GetComponent<Rigidbody>();
        if (_rigidbody == null)
        {
            _rigidbody = gameObject.AddComponent<Rigidbody>();
            _rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }

        _NavMeshAgent = GetComponent<NavMeshAgent>();

        if(_NavMeshAgent == null)
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

        _animator = GetComponentInChildren<Animator>();

        PlayerMask = LayerMask.GetMask("Player");
    }

    protected void Update()
    {
        _stateMachine.Update();
        Detect();
    }

    void Detect()
    {
        if(_stateMachine.GetState() != EntityEnum.Idle)
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
        Debug.Log("dead");
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
        Debug.Log("attack");
    }


    public void TakeDamage(float damage)
    {
        _stateMachine.SetState(EntityEnum.Hit);
    }
}
