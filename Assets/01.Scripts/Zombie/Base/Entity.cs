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
        _rigidbody.isKinematic = true;
        if (_rigidbody == null)
        {
            _rigidbody = gameObject.AddComponent<Rigidbody>();
            _rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            _rigidbody.isKinematic = true;
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

        // linq�� gpt�� ����
        ragdollColliders = GetComponentsInChildren<Collider>()
         .Where(c => c.tag != "Monster")
         .ToArray();

        ragdollRigidbodies = GetComponentsInChildren<Rigidbody>(true)
         .Where(rb => rb.tag != "Monster")
         .ToArray();


        foreach (var col in ragdollColliders)
        {
            col.enabled = false; // �ʱ⿡�� ��Ȱ��ȭ
        }

        foreach (var rb in ragdollRigidbodies)
        {
            rb.isKinematic = true; // �ʱ⿡�� Kinematic ����
        }

    }

    private void OnEnable()
    {
        SetRagdollActive(false);
    }

    protected void Update()
    {
        _stateMachine.Update();
        if (_stateMachine.GetState() == EntityEnum.Idle || _stateMachine.GetState() == EntityEnum.Walk)
        {
            Detect();
        }
    }

    void Detect()
    {
        //������ �ȳѵ�
        Collider[] targets = Physics.OverlapSphere(transform.position, baseStatus.DetectedRange, PlayerMask);

        //range for
        foreach (var target in targets)
        {
            Vector3 dirToTarget = (target.transform.position - transform.position).normalized;
            float angle = Vector3.Angle(transform.forward, dirToTarget);

            //���� 45���� ������ 45���� �� 90����
            if (angle < baseStatus.DetectedAngle)
            {
                if (Physics.Raycast(transform.position + Vector3.up * 0.5f, dirToTarget, out RaycastHit hit, baseStatus.DetectedRange))
                {
                    //�ӽ�
                    if (hit.collider.CompareTag("Player"))
                    {
                        baseStatus.DetectedLocation = hit.point; // �÷��̾� ��ġ ����
                        _stateMachine.SetState(EntityEnum.Run);
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
        if (baseStatus == null) return;
        // ���� �ݰ� ����
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, baseStatus.DetectedRange);

        // �þ߰� (viewAngle)�� �ð�ȭ (�¿�)
        Vector3 leftDir = Quaternion.Euler(0, -baseStatus.DetectedAngle, 0) * transform.forward;
        Vector3 rightDir = Quaternion.Euler(0, baseStatus.DetectedAngle, 0) * transform.forward;

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, leftDir * baseStatus.DetectedRange);
        Gizmos.DrawRay(transform.position, rightDir * baseStatus.DetectedRange);

        // (����) ���� ���� ǥ��
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, transform.forward * baseStatus.DetectedRange);
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

        DebugHelper.Log(baseStatus.CurrentHp.ToString());

        _stateMachine.SetState(EntityEnum.Hit);
    }

    public void SetRagdollActive(bool isActive)  //true�� ragdoll Ȱ��ȭ, false�� ��Ȱ��ȭ
    {
        foreach (var col in ragdollColliders)
        {
            col.enabled = isActive;
        }

        // ������ٵ� kinematic ���� (false = ���� ����, true = ��Ȱ��ȭ)
        foreach (var rb in ragdollRigidbodies)
        {
            rb.isKinematic = !isActive;
        }

        // ���� �ݶ��̴�/������ٵ� ���ų� �ѱ�
        _capsuleCollider.enabled = !isActive;

        // �׺�޽ó� �ִϸ����͵� ���°� �Ϲ����Դϴ�
        _NavMeshAgent.enabled = !isActive;
        _animator.enabled = !isActive;
    }
}
