using UnityEngine;

public class UiController : MonoBehaviour
{
    private void OnInventory()
    {
        UiManager.instance.status.SetWindow(UiCode.Inventory);
    }

    private void OnMenu()
    {

    }

    //테스트용
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I)) OnInventory();
        //else if (Input.GetKeyDown(KeyCode.Escape)) OnMenu();
    }
}
