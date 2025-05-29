using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
	[Header("Character Input Values")]
	public Vector2 move;
	public Vector2 look;
	private bool _jump;
    public bool Jump => _jump;
	private bool _sprint;
    public bool Sprint => _sprint;
	
	private bool _aim;
    public bool Aim => _aim;
    private bool _prevAim = false;
    
	private bool _attack;
    public bool Attack => _attack;
    
	private bool _canSprint = true;

	[Header("Movement Settings")] public bool analogMovement;

	[Header("Mouse Cursor Settings")]
    public bool cursorLocked = true;
	
	[SerializeField] private Player _player;
	[SerializeField] private PlayerThirdPersonController _controller;
	[SerializeField] private PlayerInteraction _playerInteraction;
	
	//테스트 코드
	public WeaponInfo[] TestInventorySelectedWeaponInfos;
	//

	private void Reset()
	{
		_controller = GetComponent<PlayerThirdPersonController>();
	}

	private void Start()
	{
		_player = Player.Instance;
        SetCursorState(cursorLocked);
	}

	private void Update()
	{
		if (_sprint)
		{
			_player.SetStamina(-(Time.deltaTime * _player.sprintStamina));

			if (_player.Stamina <= 0f)
			{
				_sprint = false;
				_canSprint = false;
			}
		}
		else if (!_canSprint && _player.Stamina > _player.maxStamina / 4f)
		{
			_canSprint = true;
		}
		
		if (_attack)
		{
            switch (Player.Instance.Equip.curWeaponType)
            {
                case PlayerWeaponType.Melee:
                    _controller.MeleeAttack();
                    _attack = false;
                    break;
                case PlayerWeaponType.Ranged:
                    _controller.RangedAttack();
                    _attack = false;
                    break;
            }
		}
	}

	public void OnMove(InputAction.CallbackContext context)
	{
		move = context.ReadValue<Vector2>();
	}
	
	public void OnLook(InputAction.CallbackContext context)
	{
        look = context.ReadValue<Vector2>();
	}
	
	public void OnJump(InputAction.CallbackContext context)
	{
        if (context.phase == InputActionPhase.Started)
        {
            if (_player.Stamina < _player.jumpStamina) return;
            _player.SetStamina(_player.jumpStamina);
            _jump = true;
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            _jump = false;
        }
	}
	
	public void OnSprint(InputAction.CallbackContext context)
	{
		if (!_canSprint) return;
		_sprint = context.ReadValueAsButton();
	}
	
	public void OnAim(InputAction.CallbackContext context)
	{
        _aim = context.ReadValueAsButton();
    }
	
	public void OnAttack(InputAction.CallbackContext context)
	{
		if (_aim)
		{
			_attack = context.ReadValueAsButton();
		}
		else
		{
			_attack = false;
		}
	}
	
	public void OnNumberInput(InputAction.CallbackContext context)
	{
		if (context.performed)
		{
			float key = context.ReadValue<float>();
			int numberPressed = Mathf.RoundToInt(key) - 1;

			// 테스트코드
			if (numberPressed >= 0 && numberPressed < TestInventorySelectedWeaponInfos.Length)
			{
				Player.Instance.Equip.EquipNew(TestInventorySelectedWeaponInfos[numberPressed]);
			}
			//
		}
	}
	
	public void OnInteraction(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
		{
			_playerInteraction.InvokePickUp();
		}
	}

    public void ToggleMouseLock()
    {
        cursorLocked = !cursorLocked;
        SetCursorState(cursorLocked);
    }
    
	//마우스 잠금처리
	private void SetCursorState(bool newState) 
	{
		Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
	}
}