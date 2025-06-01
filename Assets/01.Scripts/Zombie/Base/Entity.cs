using System.Collections;
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
    [SerializeField] protected LayerMask PlayerMask;

    [SerializeField] Collider[] ragdollColliders;
    [SerializeField] Rigidbody[] ragdollRigidbodies;
    [SerializeField] protected string PrefabName;

    protected virtual void Reset()
    {
        gameObject.tag = TagHelper.Monster;

        _rigidbody = GetComponent<Rigidbody>();
        if (_rigidbody == null)
        {
            _rigidbody = gameObject.AddComponent<Rigidbody>();
            _rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            _rigidbody.isKinematic = true;
        }
        _rigidbody.isKinematic = true;

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

        PlayerMask = LayerMask.GetMask(LayerHelper.Player);

        // linq고 gpt꺼 따옴
        ragdollColliders = GetComponentsInChildren<Collider>()
         .Where(c => c.tag != TagHelper.Monster)
         .ToArray();

        ragdollRigidbodies = GetComponentsInChildren<Rigidbody>(true)
         .Where(rb => rb.tag != TagHelper.Monster)
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

    private void OnDestroy()
    {
        CoroutineManager.Instance.UnSetAllCoroutine(this);
    }
    private void OnEnable()
    {
        SetRagdollActive(false);
        baseStatus.Init();
    }

    protected virtual void Update()
    {
        _stateMachine.Update();
        EntityEnum CurrentState = _stateMachine.GetState();
        if (CurrentState == EntityEnum.Idle || CurrentState == EntityEnum.Walk || CurrentState == EntityEnum.Hearing)
        {
            Detect();
        }
    }

    protected virtual void Detect()
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
                        _stateMachine.SetState(EntityEnum.Run);
                    }
                }
            }
        }
    }

    public EntityEnum GetState()
    {
        return _stateMachine.GetState();
    }

    public void Hearing(Vector3 _position)
    {
        baseStatus.DetectedLocation = _position;
        _stateMachine.SetState(EntityEnum.Hearing);
    }

    public virtual void Dead()
    {
        gameObject.PlayAudio("Zombie_Idle_2");
        SetRagdollActive(true);
        CoroutineManager.Instance.UnSetAllCoroutine(this);
        StartCoroutine(OnRelease());
    }

    IEnumerator OnRelease()
    {
        yield return CoroutineHelper.GetTime(5f);

        ObjectPool.Instance.Set(ContentManager.GetAsset<GameObject>(PrefabName), gameObject);
    }
    

    public void OnDisable()
    {

    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (baseStatus == null) return;
        // 감지 반경 색상
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, baseStatus.DetectedRange);

        // 시야각 (viewAngle)을 시각화 (좌우)
        Vector3 leftDir = Quaternion.Euler(0, -baseStatus.DetectedAngle, 0) * transform.forward;
        Vector3 rightDir = Quaternion.Euler(0, baseStatus.DetectedAngle, 0) * transform.forward;

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, leftDir * baseStatus.DetectedRange);
        Gizmos.DrawRay(transform.position, rightDir * baseStatus.DetectedRange);

        // (선택) 정면 방향 표시
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, transform.forward * baseStatus.DetectedRange);
    }
#endif


    public virtual void Attack_1()
    {
        _entityAttack.Attack();
    }

    public virtual void Attack_2()
    {

    }


    public virtual void TakeDamage(float damage)
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

        if (baseStatus.IsQuater() == true)
        {
            _stateMachine.SetState(EntityEnum.Hurt);
            return;
        }

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

        // 네브메시나 애니메이터도 끄는게 일반적입니다
        _NavMeshAgent.enabled = !isActive;
        _animator.enabled = !isActive;
    }
}
