using UnityEngine;

public class UiController : MonoBehaviour
{
    private void OnInventory()
    {
        var isActive = UiManager.instance.status.inventory.activeSelf;

        UiManager.instance.shader.SetActive(!isActive);
        UiManager.instance.status.inventory.SetActive(!isActive);

        if (isActive) UiManager.instance.status.itemInfo.SetActive();
    }

    private void OnMenu()
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
