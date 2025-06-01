using System.Collections;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerThirdPersonController : MonoBehaviour
{
    private const float _threshold = 0.01f;

    // =================== 인스펙터에서 설정하는 값들 ===================
    [Header("Player")] //이동관련
    public float MoveSpeed = 10f;

    public float SprintSpeed = 30f;

    //마우스 감도
    [Range(0.0f, 10.0f)] public float mouseSesitive = 5f;

    //플레이어 회전 부드러움값
    [Range(0.0f, 0.3f)] public float RotationSmoothTime = 0.1f;

    //플레이어 속도(달리기할때 달리기 풀때) 변화 값
    public float SpeedChangeRate = 10.0f;

    [Space(10)] //점프높이
    public float JumpHeight = 4f;

    [Space(10)] //점프시 떨어지는 애니메이션 실행 쿨타임
    public float JumpTimeout = 0.50f;

    public float FallTimeout = 0.15f;

    [Header("Player Grounded")] //지상 판정 관련
    public bool Grounded = true;

    //지상 판정 가상 콜라이더 값들
    public float GroundedOffset = -0.14f;

    public float GroundedRadius = 0.28f;

    //지상 판정 레이어
    public LayerMask GroundLayers;

    [Header("Cinemachine")] //카메라 관련
    //카메라가 실제로 회전시킬 오브젝트
    public GameObject CinemachineCameraTarget;

    //버추얼 카메라 관련
    public CinemachineVirtualCamera VirtualCamera; //버추얼 카메라 본채

    public Cinemachine3rdPersonFollow Follow; //버추얼 카메라의 3인칭 설정 내부 컴포넌트

    //3인칭, 조준시
    public Vector3 TPSCamPosition;
    public Vector3 AimCamPosition = new(2.5f, -0.3f, 2.5f);

    [Range(0.0f, 0.5f)] public float TPSDamping = 0.5f;
    [Range(0.0f, 0.5f)] public float aimDamping;

    //카메라 위아래 회전 제한값
    public float TopClamp = 70.0f;
    public float BottomClamp = -30.0f;

    //카메라 상하 회전 잠금
    public bool AimLock;

    [Header("Componetns")]
    //컴포넌트들
    [SerializeField]
    private PlayerInput _playerInput;

    [SerializeField] private PlayerInputs _input;
    [SerializeField] private GameObject _mainCamera;
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody _rigidbody;

    //애니메이션 플로트,불,트리거값
    private float _animationBlend;
    private int _animIDAim;
    private int _animIDAttack;
    private int _animIDFreeFall;
    private int _animIDGrounded;
    private int _animIDJump;
    private int _animIDMotionSpeed;
    private int _animIDSpeed;
    private int _animIDUse;

    // =================== 내부 사용값들 ===================
    //카메라 회전관련
    private float _cinemachineTargetPitch; //위아래 회전
    private float _cinemachineTargetYaw; //좌우 회전

    //떨어지는 애니메이션용 타이머
    private float _fallTimeoutDelta;

    //점프 쿨타임
    private float _jumpTimeoutDelta;
    private float _rotationVelocity;

    //플레이어 속성값
    private float _speed;
    private float _targetRotation;
    private float _verticalVelocity;

    //줌인 줌아웃 코루틴
    private Coroutine _zoomInCoroutine;
    private Coroutine _zoomOutCoroutine;

    //플레이어 조작장치가 키보드 마우스 판단.
    private bool IsCurrentDeviceMouse => _playerInput.currentControlScheme == "KeyboardMouse";

    private void Reset()
    {
        CinemachineCameraTarget = GameObject.Find("CameraRoot");
        GroundLayers = LayerMask.GetMask("Default");
        _playerInput = GetComponent<PlayerInput>();
        _rigidbody = GetComponent<Rigidbody>();
        _input = GetComponent<PlayerInputs>();
        _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        VirtualCamera = GameObject.Find("PlayerFollowCamera").GetComponent<CinemachineVirtualCamera>();
        Follow = VirtualCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
        TPSCamPosition = Follow.ShoulderOffset;
        _animator = GetComponent<Animator>();
    }

    //컴포넌트 값 초기화
    private void Start()
    {
        _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;

        AssignAnimationIDs();

        _jumpTimeoutDelta = JumpTimeout;
        _fallTimeoutDelta = FallTimeout;
    }

    private void Update()
    {
        JumpAndGravity();
        GroundedCheck();
        AimSwitch(_input.Aim);
        if (UiManager.instance.isActive)
        {
            _input.look = Vector2.zero;
        }
        CameraRotation();
    }

    private void FixedUpdate()
    {
        if (UiManager.instance.isActive)
        {
            _input.move = Vector2.zero;
        }
        Move();
    }

    //씬에서 플레이어 선택시 기즈모 그리기
    private void OnDrawGizmosSelected()
    {
        Color transparentGreen = new(0.0f, 1.0f, 0.0f, 0.35f);
        Color transparentRed = new(1.0f, 0.0f, 0.0f, 0.35f);

        if (Grounded)
        {
            Gizmos.color = transparentGreen;
        }
        else
        {
            Gizmos.color = transparentRed;
        }

        Gizmos.DrawSphere(
            new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z),
            GroundedRadius);
    }

    //애니메이션 등록함수
    private void AssignAnimationIDs()
    {
        _animIDSpeed = Animator.StringToHash("Speed");
        _animIDGrounded = Animator.StringToHash("Grounded");
        _animIDJump = Animator.StringToHash("Jump");
        _animIDFreeFall = Animator.StringToHash("FreeFall");
        _animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
        _animIDAim = Animator.StringToHash("Aim");
        _animIDAttack = Animator.StringToHash("Attack");
        _animIDUse = Animator.StringToHash("Use");
    }

    //바닥 판정 함수(바닥판정을 위한 원의 중심위치 정하고 원이랑 바닥레이어랑 충돌하면 바닥판정
    private void GroundedCheck()
    {
        Vector3 spherePosition = new(transform.position.x, transform.position.y - GroundedOffset,
            transform.position.z);
        Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
            QueryTriggerInteraction.Ignore);

        _animator.SetBool(_animIDGrounded, Grounded);
    }

    //카메라 회전. 밑과 위를 제한하는 값으로 최대치 제한
    private void CameraRotation()
    {
        float deltaTimeMultiplier = IsCurrentDeviceMouse ? mouseSesitive : Time.fixedDeltaTime;

        if (AimLock)
        {
            _cinemachineTargetPitch = Mathf.Lerp(_cinemachineTargetPitch, 10f, Time.fixedDeltaTime * 6f);
        }
        else
        {
            if (_input.look.sqrMagnitude >= _threshold && !AimLock)
            {
                _cinemachineTargetPitch += _input.look.y * deltaTimeMultiplier;
            }
        }

        _cinemachineTargetYaw += _input.look.x * deltaTimeMultiplier;


        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

        CinemachineCameraTarget.transform.rotation =
            Quaternion.Euler(_cinemachineTargetPitch, _cinemachineTargetYaw, 0.0f);
    }


    public void StopAnim()
    {
        _animator.Play("Idle Walk Run Blend");
    }

    //이동.
    private void Move()
    {
        //========== 이동값 받음 ==========
        float targetSpeed = _input.Sprint ? SprintSpeed : MoveSpeed;
        if (_input.move == Vector2.zero)
        {
            targetSpeed = 0.0f;
        }

        float currentHorizontalSpeed = new Vector3(_rigidbody.velocity.x, 0.0f, _rigidbody.velocity.z).magnitude;
        float speedOffset = 0.1f;
        float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

        if (Mathf.Abs(currentHorizontalSpeed - targetSpeed) > speedOffset)
        {
            _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                Time.fixedDeltaTime * SpeedChangeRate);
            _speed = Mathf.Round(_speed * 1000f) / 1000f;
        }
        else
        {
            _speed = targetSpeed;
        }

        _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.fixedDeltaTime * SpeedChangeRate);
        if (_animationBlend < 0.01f)
        {
            _animationBlend = 0f;
        }

        Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;
        Vector3 targetDirection = Vector3.zero;

        //========== 카메라 회전에 따라 플레이어 회전 시키는 부분 ============
        float rotation;
        if (AimLock)
        {
            Follow.Damping.x = aimDamping;
            _targetRotation = _mainCamera.transform.eulerAngles.y;
            targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * inputDirection;

            rotation = _targetRotation;
        }
        else
        {
            Follow.Damping.x = TPSDamping;
            _targetRotation = (Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg)
                              + _mainCamera.transform.eulerAngles.y;
            targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

            rotation = Mathf.SmoothDampAngle(
                transform.eulerAngles.y,
                _targetRotation,
                ref _rotationVelocity,
                RotationSmoothTime
            );
        }

        //========== 회전 및 이동 적용 ==========
        transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        Vector3 move = targetDirection.normalized * (_speed * Time.fixedDeltaTime);
        Vector3 newPosition = _rigidbody.position + move;
        _rigidbody.MovePosition(newPosition);

        _animator.SetFloat(_animIDSpeed, _animationBlend);
        _animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
    }


    //점프.
    private void JumpAndGravity()
    {
        if (Grounded)
        {
            //땅에 닿아있을때
            _fallTimeoutDelta = FallTimeout;


            _animator.SetBool(_animIDJump, false);
            _animator.SetBool(_animIDFreeFall, false);

            if (_input.Jump && _jumpTimeoutDelta <= 0.0f)
            {
                Vector3 velocity = _rigidbody.velocity;
                velocity.y = Mathf.Sqrt(JumpHeight * 10f);
                _rigidbody.velocity = velocity;


                _animator.SetBool(_animIDJump, true);
            }

            if (_jumpTimeoutDelta >= 0.0f)
            {
                _jumpTimeoutDelta -= Time.deltaTime;
            }
        }
        else
        {
            //땅에 닿지 않았을때
            _jumpTimeoutDelta = JumpTimeout;

            if (_fallTimeoutDelta >= 0.0f)
            {
                _fallTimeoutDelta -= Time.deltaTime;
            }
            else
            {
                _animator.SetBool(_animIDFreeFall, true);
            }
        }
    }

    //카메라 회전에 사용하는 값제한 함수
    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f)
        {
            lfAngle += 360f;
        }

        if (lfAngle > 360f)
        {
            lfAngle -= 360f;
        }

        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

    //조준 구분 함수
    private void AimSwitch(bool isAimed)
    {
        Vector3 targetOffset = isAimed ? AimCamPosition : TPSCamPosition;

        if (isAimed)
        {
            if (_zoomInCoroutine != null)
            {
                return;
            }

            if (_zoomOutCoroutine != null)
            {
                return;
            }

            _zoomInCoroutine = StartCoroutine(ZoomIn(targetOffset));
        }
        else
        {
            if (_zoomInCoroutine != null)
            {
                return;
            }

            if (_zoomOutCoroutine != null)
            {
                return;
            }

            _zoomOutCoroutine = StartCoroutine(ZoomOut(targetOffset));
        }

        _animator.SetBool(_animIDAim, isAimed);
    }

    private IEnumerator ZoomIn(Vector3 targetOffset)
    {
        if (Follow.ShoulderOffset == targetOffset)
        {
            _zoomInCoroutine = null;
            yield return null;
        }

        AimLock = true;

        Vector3 startOffset = Follow.ShoulderOffset;
        float elapsedTime = 0f;
        float duration = 0.3f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            Follow.ShoulderOffset = Vector3.Lerp(startOffset, targetOffset, t);
            yield return null;
        }

        Follow.ShoulderOffset = targetOffset;

        yield return new WaitForSeconds(0.3f);

        _zoomInCoroutine = null;
    }

    private IEnumerator ZoomOut(Vector3 targetOffset)
    {
        if (Follow.ShoulderOffset == targetOffset)
        {
            _zoomOutCoroutine = null;
            yield return null;
        }

        AimLock = false;

        Vector3 startOffset = Follow.ShoulderOffset;
        float elapsedTime = 0f;
        float duration = 0.3f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / duration);
            Follow.ShoulderOffset = Vector3.Lerp(startOffset, targetOffset, t);
            yield return null;
        }

        Follow.ShoulderOffset = targetOffset;

        yield return new WaitForSeconds(0.3f);

        _zoomOutCoroutine = null;
    }


    public void MeleeAttack()
    {
        _animator.SetTrigger(_animIDAttack);
    }

    public void RangedAttack()
    {
        (Vector3 rayOrigin, Vector3 direction) = Player.Instance.Equip.curWeaponData.GetBulletStartPoint();
        Physics.Raycast(rayOrigin, direction, out RaycastHit hit, Player.Instance.Equip.curWeaponData.Range);
#if UNITY_EDITOR
        Debug.DrawRay(rayOrigin, direction, Color.red);
#endif
        if (hit.collider != null && hit.collider.TryGetComponent(out IDamagable damagable))
        {
            damagable.TakeDamage(Player.Instance.Equip.curWeapon.power);
        }
    }

    public void UseItem()
    {
        ItemManager.Instance.Inventory.UseItem(Player.Instance.Equip.curEquip.itemId);
        _animator.SetTrigger(_animIDUse);
        Player.Instance.Sound.HealSound();
    }
}