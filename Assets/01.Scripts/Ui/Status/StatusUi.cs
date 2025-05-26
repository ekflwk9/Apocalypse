using System.Collections.Generic;
using UnityEngine;

public class StatusUi : MonoBehaviour
{
    Dictionary<UiCode, GameObject> window = new Dictionary<UiCode, GameObject>();

    private Slot[] inventorySlot;
    private Slot[] storageSlot;
    private Slot[] statusSlot;

    private void Awake()
    {
        
    }

    private void FindInventorySlot(string _parentName)
    {
        var parent = Helper.FindChild(this.transform, _parentName);

    }

    private void ASD()
    {
        var tempList = new List<Slot>();

        for (int i = 0; i < inventorySlot.Length; i++)
        {

        }
    }


    /// <summary>
    /// �������� ��� ���� / �������� ��� ����
    /// </summary>
    /// <param name="_uiType"></param>
    public void SetWindow(UiCode _uiType)
    {
        if (window.ContainsKey(_uiType))
        {
            var isActive = window[(_uiType)].activeSelf;
            window[_uiType].SetActive(!isActive);
        }

        else
        {
            DebugHelper.Log($"{_uiType}�̶�� ���� �������� ����");
        }
    }

   
    /// <summary>
    /// Ui Ȱ��ȭ ���¸� ���� ������
    /// </summary>
    /// <param name="_uiType"></param>
    /// <param name="_isActive"></param>
    public void SetWindow(UiCode _uiType, bool _isActive)
    {
        if (window.ContainsKey(_uiType)) window[_uiType].SetActive(_isActive);
        else DebugHelper.Log($"{_uiType}�̶�� ���� �������� ����");
    }
}
