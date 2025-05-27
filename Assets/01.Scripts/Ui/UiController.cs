using UnityEngine;

public class UiController : MonoBehaviour
{
    /// <summary>
    /// �κ��丮�� Ȱ��ȭ�ϴ� �޼���
    /// </summary>
    public void OnInventory()
    {
        var isActive = UiManager.instance.status.inventory.activeSelf;

        UiManager.instance.shader.SetActive(!isActive);
        UiManager.instance.status.inventory.SetActive(!isActive);

        if (isActive)
        {
            UiManager.instance.touch.SetTouch(false);
            UiManager.instance.status.itemInfo.SetOff();
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
