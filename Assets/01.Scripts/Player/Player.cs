using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }
    
    [Header("State")]
    public float maxHealth = 100f;
    public float maxStamina = 100f;
    
    [SerializeField] private float _health;
    [SerializeField] private float _stamina;
    
    public float passiveStamina = 5f;

    [Header("Stamina Use")]
    public float sprintStamina = 5f;
    public float jumpStamina = 10f;
    public float staminaRegenCooldown = 5f;
    
    private bool _staminaRegen;
    private Coroutine _staminaRegenCoroutine;

    public float Health
    {
        get => _health;
        set => _health = Mathf.Clamp(value, 0, maxHealth);
    }

    public float Stamina
    {
        get => _stamina;
        set
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
}