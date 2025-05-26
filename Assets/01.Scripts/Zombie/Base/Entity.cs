using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



public class Entity : MonoBehaviour
{


    [SerializeField] protected Rigidbody _rigidbody;
    protected EntityStateMachine _stateMachine;
    public BaseStatus baseStatus;
    public NavMeshAgent _NavMeshAgent;
    public Animator _animator;

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

        _animator = GetComponentInChildren<Animator>();
    }

    protected void Update()
    {
        _stateMachine.Update();
        Detect();
    }

    [SerializeField] LayerMask mask;
    void Detect()
    {
        if(_stateMachine.GetState() != EntityEnum.Idle)
        {
            return;
        }

        //������ �ȳѵ�
        Collider[] targets = Physics.OverlapSphere(transform.position, 20f, mask);

        //range for
        foreach (var target in targets)
        {
            Vector3 dirToTarget = (target.transform.position - transform.position).normalized;
            float angle = Vector3.Angle(transform.forward, dirToTarget);

            //���� 45���� ������ 45���� �� 90����
            if (angle < 45f) 
            {
                if (Physics.Raycast(transform.position + Vector3.up * 0.5f, dirToTarget, out RaycastHit hit, 20f))
                {
                    //�ӽ�
                    if (hit.collider.CompareTag("Player"))
                    {
                        _stateMachine.SetState(EntityEnum.Run);
                        Debug.Log("Ȯ��");
                    }
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        // ���� �ݰ� ����
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 20);

        // �þ߰� (viewAngle)�� �ð�ȭ (�¿�)
        Vector3 leftDir = Quaternion.Euler(0, -90f / 2f, 0) * transform.forward;
        Vector3 rightDir = Quaternion.Euler(0, 90f / 2f, 0) * transform.forward;

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, leftDir * 20);
        Gizmos.DrawRay(transform.position, rightDir * 20);

        // (����) ���� ���� ǥ��
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, transform.forward * 20);
    }

}
