using System.Collections;
using Cinemachine;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI;

public interface IDamagable
{
    public void TakeDamage(float damage);
}

public class Player : MonoBehaviour, IDamagable
{
    public static Player Instance { get; private set; }

    public PlayerEquip Equip;
    public PlayerSound Sound;
    public CinemachineVirtualCamera cinemachineCamera;
    public CinemachineBasicMultiChannelPerlin perlin;
    public PlayerThirdPersonController ThirdPersonController;

    [Header("State")]
    public float maxHealth = 100f;
    public float maxStamina = 100f;

    [SerializeField] private float _health;
    [SerializeField] private float _stamina;
    [SerializeField] private int _defence;
    [SerializeField] private int _gold;
    [SerializeField] private int _level;
    [SerializeField] private int _weight;
    [SerializeField] private int _maxWeight = 50;

    public float passiveStamina = 5f;

    [Header("Stamina Use")]
    public float sprintStamina = 5f;
    public float jumpStamina = 10f;
    public float staminaRegenCooldown = 5f;

    [Header("Animations")]
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private BoxCollider meleeCollider;

    [SerializeField] private Animator _animator;
    private int _animIDDamage;
    private int _animIDDead;

    private Coroutine _damagedCoroutine;
    private Coroutine _staminaRegenCoroutine;

    private bool _staminaRegen;
    private bool _toggleMelee;
    public bool Damaged { get; private set; }

    public bool Dead { get; private set; }

    public float Health
    {
        get => _health;

        private set
        {
            float changedValue = Mathf.Clamp(value, 0, maxHealth);

            if (changedValue < _health)
            {
                Damaged = true;
                _health = changedValue;
            }

            else
            {
                _health = value;
            }
        }
    }

    public float Stamina
    {
        get => _stamina;
        private set
        {
            float changedValue = Mathf.Clamp(value, 0, maxStamina);

            if (changedValue < _stamina)
            {
                if (_staminaRegenCoroutine != null)
                {
                    StopCoroutine(_staminaRegenCoroutine);
                }

                _staminaRegenCoroutine = StartCoroutine(StaminaRegenDelay());
            }

            _stamina = changedValue;

            UiManager.instance.play.stamina.SetSlider(value / 100f);
        }
    }

    public int Defence { get; private set; }

    public int Gold
    {
        get => _gold;
        private set
        {
            if (value <= 0) { _gold = 0; }
            else { _gold = value; }
        }
    }

    public int Level
    {
        get => _level;
        private set
        {
            if (value <= 0) { _level = 0; }
            else { _level = value; }
        }
    }

    public int Weight
    {
        get => _weight;
        private set
        {
            if (value <= 0) { _weight = 0; }
            else { _weight = value; }
        }
    }

    public int MaxWeight => _maxWeight;

    private void Awake()
    {
        //항상 구현하던 방식으로 구현하였기에 필요없다면 삭제.
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject.transform.parent.gameObject);
        }
        else
        {
            Destroy(gameObject.transform.parent.gameObject);
        }
    }

    private void Reset()
    {
        cinemachineCamera = FindObjectOfType<CinemachineVirtualCamera>();
        perlin = cinemachineCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
        Equip = GetComponentInChildren<PlayerEquip>();
        meleeCollider = GetComponentInChildren<BoxCollider>();
        meleeCollider.enabled = false;
        Sound = GetComponent<PlayerSound>();
        ThirdPersonController = this.TryFindChildComponent<PlayerThirdPersonController>();
    }

    private void Start()
    {
        Health = maxHealth;
        Stamina = maxStamina;
        _animIDDamage = Animator.StringToHash("Damage");
        _animIDDead = Animator.StringToHash("Dead");
    }

    public void OnStart()
    {
        Health = maxHealth;
        UiManager.instance.play.health.SetSlider(Health / 100f);
        Stamina = maxStamina;
        Dead = false;
        _rigidbody.isKinematic = false;
        _animator.SetBool(_animIDDead, false);
    }

    private void Update()
    {
        if (_staminaRegen)
        {
            Stamina += Time.deltaTime * passiveStamina;

            if (Stamina >= maxStamina)
            {
                _staminaRegen = false;
            }
        }
    }

    public void TakeDamage(float damage)
    {
        if (Dead) return;

        if (Defence > 0)
        {
            Defence -= 1;

            ItemManager.Instance.Inventory.Defense(Defence);
            UiManager.instance.play.defense.SetSlider(Defence);
            return;
        }

        Health -= damage;
        UiManager.instance.hitUi.Show(true);
        UiManager.instance.play.health.SetSlider(Health / 100f);

        if (Health <= 0)
        {
            Dead = true;
            _rigidbody.isKinematic = true;

            UiManager.instance.status.dead.gameObject.SetActive(true);
            StopAllCoroutines();
        }

        DamageAnimation();

        if (_damagedCoroutine != null)
        {
            StopCoroutine(_damagedCoroutine);
        }

        _damagedCoroutine = StartCoroutine(DamagedCoroutine());
    }

    private IEnumerator StaminaRegenDelay()
    {
        _staminaRegen = false;

        yield return new WaitForSeconds(staminaRegenCooldown);

        _staminaRegen = true;

        _staminaRegenCoroutine = null;
    }

    private void DamageAnimation()
    {
        if (Dead)
        {
            _animator.SetBool(_animIDDead, Dead);
        }
        else if (Damaged)
        {
            _animator.SetTrigger(_animIDDamage);
            Damaged = false;
        }
    }

    private IEnumerator DamagedCoroutine()
    {
        perlin.m_FrequencyGain = 20f;

        yield return new WaitForSeconds(0.2f);

        perlin.m_FrequencyGain = 0f;
    }

    public void Heal(float heal)
    {
        float healedHealth = Health + heal;
        Health = healedHealth;
    }

    public void SetStamina(float stamina)
    {
        Stamina += stamina;
    }

    public void SetDefence(int defence)
    {
        Defence += defence;
    }

    public void SetGold(int money)
    {
        Gold += money;
    }

    public void SetLevel(int level)
    {
        Level += level;
    }

    public void SetWeight(int weight)
    {
        Weight += weight;
    }

    public void ToggleMeleeCollider()
    {
        if (Equip.EquipMelee)
        {
            _toggleMelee = !_toggleMelee;
            meleeCollider.enabled = _toggleMelee;
        }
    }

    public void ResetPlayer()
    {
        Defence = 0;
        Health = 100;
        Stamina = 0;
    }
}