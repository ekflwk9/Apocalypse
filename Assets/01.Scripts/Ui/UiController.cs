using UnityEngine;

public class UiController : MonoBehaviour
{
    /// <summary>
    /// �κ��丮�� Ȱ��ȭ�ϴ� �޼���
    /// </summary>
    public void OnInventory()
    {
        var isActive = UiManager.instance.status.inventory.activeSelf;
        var status = UiManager.instance.status;

        UiManager.instance.shader.SetActive(!isActive);
        status.inventory.SetActive(!isActive);

        if (isActive)
        {
            status.drag.OnEndDrag();
            status.itemInfo.SetOff();
            UiManager.instance.touch.SetTouch(false);
        }
    }

    /// <summary>
    /// ���� â�� Ȱ��ȭ �ϴ� �޼���
    /// </summary>
    public void OnMenu()
    {
        //var isActive = UiManager.instance.menu.activeSelf;

        //UiManager.instance.shader.SetActive(!isActive);
        //UiManager.instance.menu
    }

    //�׽�Ʈ��
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I)) OnInventory();
        //else if (Input.GetKeyDown(KeyCode.Escape)) OnMenu();
    }
}
