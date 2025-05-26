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
		jump = value.isPressed;
	}

	//달리기 입력(PlayerInput에서 설정)
	public void OnSprint(InputValue value) 
	{
		sprint = value.isPressed;
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