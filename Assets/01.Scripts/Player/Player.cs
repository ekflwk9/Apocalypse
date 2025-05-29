using System;
using System.Collections;
using UnityEngine;

public interface IDamagable
{
    public void TakeDamage(float damage);
}

public class Player : MonoBehaviour, IDamagable
{
    public static Player Instance { get; private set; }
    public PlayerEquip Equip;
    
    [Header("State")]
    public float maxHealth = 100f;
    public float maxStamina = 100f;

    [SerializeField] private float _health;
    [SerializeField] private float _stamina;
    [SerializeField] private int _gold;
    [SerializeField] private int _level;
    [SerializeField] private int _weight;

    public float passiveStamina = 5f;

    [Header("Stamina Use")] public float sprintStamina = 5f;
    public float jumpStamina = 10f;
    public float staminaRegenCooldown = 5f;

    private bool _staminaRegen;
    private Coroutine _staminaRegenCoroutine;

    public bool _damaged = false;

    public float Health
    {
        get => _health;
        private set
        {
            float changedValue = Mathf.Clamp(value, 0, maxHealth);

            if (changedValue < _health)
            {
                _damaged = true;
            }
            
            _health = changedValue;
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
                    StopCoroutine(_staminaRegenCoroutine);

                _staminaRegenCoroutine = StartCoroutine(StaminaRegenDelay());
            }

            _stamina = changedValue;
        }
    }

    public int Gold
    {
        get
        {
            return _gold;
        }
        private set
        {
            if (value <= 0)
            {
                _gold = 0;
            }
            else
            {
                _gold = value;
            }
        }
    }

    public int Level
    {
        get
        {
            return _level;
        }
        private set
        {
            if (value <= 0)
            {
                _level = 0;
            }
            else
            {
                _level = value;
            }
        }
    }

    public int Weight
    {
        get
        {
            return _weight;
        }
        private set
        {
            if (value <= 0)
            {
                _weight = 0;
            }
            else
            {
                _weight = value;
            }
        }
    }
    
    private void Reset()
    {
        Equip = GetComponent<PlayerEquip>();
    }

    private void Awake()
    {
        //항상 구현하던 방식으로 구현하였기에 필요없다면 삭제.
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        Health = maxHealth;
        Stamina = maxStamina;
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

    private IEnumerator StaminaRegenDelay()
    {
        _staminaRegen = false;

        yield return new WaitForSeconds(staminaRegenCooldown);

        _staminaRegen = true;

        _staminaRegenCoroutine = null;
    }

    public void TakeDamage(float damage)
    {
        Health -= damage;
    }

    public void Heal(float heal)
    {
        float changedHealth = Health + heal;
        Health = Mathf.Lerp(Health, changedHealth, 3f);
    }

    public void SetStamina(float stamina)
    {
        Stamina += stamina;
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
}