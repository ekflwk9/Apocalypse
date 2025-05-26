using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
	[Header("Character Input Values")] public Vector2 move;
	public Vector2 look;
	public bool jump;
	public bool sprint;

	[Header("Movement Settings")] public bool analogMovement;

	[Header("Mouse Cursor Settings")] public bool cursorLocked = true;
	public bool cursorInputForLook = true;
	
	public void OnMove(InputValue value) //WASD입력 받을때(PlayerInput에서 설정)
	{
		move = value.Get<Vector2>();
	}

	public void OnLook(InputValue value) //마우스위치 따라가기(PlayerInput에서 설정)
	{
		if (cursorInputForLook)
		{
			look = value.Get<Vector2>();
		}
	}

	public void OnJump(InputValue value) //점프 입력(PlayerInput에서 설정)
	{
		jump = value.isPressed;
	}

	public void OnSprint(InputValue value) //달리기 입력(PlayerInput에서 설정)
	{
		sprint = value.isPressed;
	}
	
	private void OnApplicationFocus(bool hasFocus) //화면 집중(에디터에선 개임씬이 눌려있는지, 애플리케이션의 경우 해당 창이 눌려있는지)
	{
		SetCursorState(cursorLocked);
	}

	private void SetCursorState(bool newState) //마우스 잠금처리
	{
		Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
	}
}