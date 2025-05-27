using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
	[Header("Character Input Values")]
	public Vector2 move;
	public Vector2 look;
	public bool jump;
	public bool sprint;
	public bool aim;
	public bool attack;

	private bool _canSprint = true;

	[Header("Movement Settings")] public bool analogMovement;

	[Header("Mouse Cursor Settings")] public bool cursorLocked = true;
	public bool cursorInputForLook = true;
	
	private Player _player;

	private void Start()
	{
		_player = Player.Instance;
	}

	private void Update()
	{
		if (sprint)
		{
			_player.Stamina -= Time.deltaTime * _player.sprintStamina;

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
	}

	//WASD입력 받을때(PlayerInput에서 설정)
	public void OnMove(InputValue value) 
	{
		move = value.Get<Vector2>();
	}

	//마우스위치 따라가기(PlayerInput에서 설정)
	public void OnLook(InputValue value) 
	{
		if (cursorInputForLook)
		{
			look = value.Get<Vector2>();
		}
	}

	//점프 입력(PlayerInput에서 설정)
	public void OnJump(InputValue value) 
	{
		if (!jump && value.isPressed)
		{
			if(_player.Stamina < _player.jumpStamina) return;
			_player.Stamina -= _player.jumpStamina;
			jump = true;
		}
	}

	//달리기 입력(PlayerInput에서 설정)
	public void OnSprint(InputValue value) 
	{
		if(!_canSprint) return;
		sprint = value.isPressed;
	}

	public void OnAim(InputValue value)
	{
		aim = value.isPressed;
	}

	public void OnAttack(InputValue value)
	{
		if (value.isPressed && aim && !attack)
		{
			attack = true;
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