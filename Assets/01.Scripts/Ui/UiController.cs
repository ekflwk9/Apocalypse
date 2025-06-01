using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class UiController : MonoBehaviour
{
    /// <summary>
    /// 인벤토리를 활성화하는 메서드
    /// </summary>
    public void OnInventory(InputAction.CallbackContext context)
    {
        //메뉴가 비활성화인 상태인 경우에만
        if (!UiManager.instance.menu.menuWindow.gameObject.activeSelf && context.started)
        {
            var isActive = UiManager.instance.status.inventory.gameObject.activeSelf;
            var status = UiManager.instance.status;

            UiManager.instance.fade.OnFade();
            UiManager.instance.SetActive(!isActive);
            UiManager.instance.shaderEffect.SetActive(!isActive);

            status.inventory.gameObject.SetActive(!isActive);
            status.equipped.SetActive(!isActive);

            if (isActive)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

                status.drag.OnEndDrag();
                status.itemInfo.SetOff();

                UiManager.instance.touch.SetTouch(false);
                status.UpdateFarmingSlot();

                var farming = status.farming.gameObject;
                if (farming.gameObject.activeSelf) farming.SetActive(false);
            }

            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }

    /// <summary>
    /// 설정 창을 활성화 하는 메서드
    /// </summary>
    public void OnMenu(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            var scene = SceneManager.GetActiveScene().name.Contains("Loby");
            var menu = UiManager.instance.menu;
            var isActive = menu.menuWindow.activeSelf;

            UiManager.instance.fade.OnFade();
            menu.menuWindow.SetActive(!isActive);

            if (!UiManager.instance.status.inventory.gameObject.activeSelf && !scene)
            {
                UiManager.instance.SetActive(!isActive);
                UiManager.instance.shaderEffect.SetActive(!isActive);

                Cursor.lockState = isActive ? CursorLockMode.Locked : CursorLockMode.None;
                Cursor.visible = !isActive;
            }
        }
    }

    //********************테스트용
    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.I)) OnInventory();
    //    else if (Input.GetKeyDown(KeyCode.Escape)) OnMenu();
    //    else if (Input.GetKeyDown(KeyCode.O))
    //    {
    //        UiManager.instance.status.shop.SetActive(!UiManager.instance.status.shop.gameObject.activeSelf);
    //    }
    //}
}
