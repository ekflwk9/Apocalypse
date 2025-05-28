using UnityEngine;

public class UiController : MonoBehaviour
{
    /// <summary>
    /// 인벤토리를 활성화하는 메서드
    /// </summary>
    public void OnInventory()
    {
        var isActive = UiManager.instance.status.inventory.activeSelf;
        var status = UiManager.instance.status;

        UiManager.instance.shader.SetActive(!isActive);
        status.inventory.SetActive(!isActive);
        status.equipped.SetActive(!isActive);

        if (isActive)
        {
            status.drag.OnEndDrag();
            status.itemInfo.SetOff();
            UiManager.instance.touch.SetTouch(false);
        }
    }

    /// <summary>
    /// 설정 창을 활성화 하는 메서드
    /// </summary>
    public void OnMenu()
    {
        //var isActive = UiManager.instance.menu.activeSelf;

        //UiManager.instance.shader.SetActive(!isActive);
        //UiManager.instance.menu
    }

    //테스트용
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I)) OnInventory();
        //else if (Input.GetKeyDown(KeyCode.Escape)) OnMenu();
    }
}
