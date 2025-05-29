using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
	public event Action<bool> AimEvent;
	[Header("Character Input Values")]
	public Vector2 move;
	public Vector2 look;
	public bool jump;
	public bool sprint;
	
	private bool aim;
	public bool attack;

	private bool _canSprint = true;

	[Header("Movement Settings")] public bool analogMovement;

	[Header("Mouse Cursor Settings")] public bool cursorLocked = true;
	public bool cursorInputForLook = true;
	
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
	}

	private void Update()
	{
		if (sprint)
		{
			_player.SetStamina(-(Time.deltaTime * _player.sprintStamina));

			if (_player.Stamina <= 0f)
			{
				sprint = false;
				_canSprint = false;
			}
		}
		else if (!_canSprint && _player.Stamina > _player.maxStamina / 4f)
		{
			_canSprint = true;
		}
		
		if (attack)
		{
			_controller.Attack();
		}
	}

	public void OnMove(InputAction.CallbackContext context)
	{
		move = context.ReadValue<Vector2>();
	}
	
	public void OnLook(InputAction.CallbackContext context)
	{
		if (cursorInputForLook)
		{
			look = context.ReadValue<Vector2>();
		}
	}
	
	public void OnJump(InputAction.CallbackContext context)
	{
		if (!jump && context.performed)
		{
			if (_player.Stamina < _player.jumpStamina) return;
			_player.SetStamina(_player.jumpStamina);
			jump = true;
		}
	}
	
	public void OnSprint(InputAction.CallbackContext context)
	{
		if (!_canSprint) return;
		sprint = context.ReadValueAsButton();
	}
	
	public void OnAim(InputAction.CallbackContext context)
	{
		aim = context.ReadValueAsButton();
		AimEvent?.Invoke(aim);
	}
	
	public void OnAttack(InputAction.CallbackContext context)
	{
		if (aim)
		{
			attack = context.ReadValueAsButton();
		}
		else
		{
			attack = false;
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
		if (context.performed)
		{
			_playerInteraction.InvokePickUp();
		}
	}

	//화면 집중(에디터에선 개임씬이 눌려있는지, 애플리케이션의 경우 해당 창이 눌려있는지)
	private void OnApplicationFocus(bool hasFocus)
	{
		SetCursorState(cursorLocked);
	}

	//마우스 잠금처리
	private void SetCursorState(bool newState) 
	{
		Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
	}
}