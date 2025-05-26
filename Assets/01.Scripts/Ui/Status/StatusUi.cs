using System;
using System.Collections.Generic;
using UnityEngine;

public class StatusUi : MonoBehaviour
{
    private Dictionary<UiCode, GameObject> window = new Dictionary<UiCode, GameObject>();

    private Slot[] inventorySlot;
    private Slot[] storageSlot;
    private Slot[] statusSlot;

    private void Awake()
    {
        Find("Inventory", out statusSlot);
        Find("Storage", out statusSlot);
        Find("PlayerInfo", out statusSlot);
    }

    private void Find(string _parentName, out Slot[] _slotArray)
    {
        var parent = Helper.FindChild(this.transform, _parentName);

        if (Enum.TryParse<UiCode>(_parentName, out var type))
        {
            window.Add(type, parent.gameObject);
        }

        else
        {
            DebugHelper.Log($"{_parentName}�̶�� Ÿ���� UiCode�� �������� ����");
            _slotArray = null;
            return;
        }

        var childCount = parent.childCount;
        var tempList = new List<Slot>();

        for (int i = 0; i < childCount; i++)
        {
            var child = parent.GetChild(i);
            if (child.TryGetComponent<Slot>(out var target)) tempList.Add(target);
        }

        _slotArray = new Slot[tempList.Count];
        tempList.CopyTo(0, _slotArray, 0, tempList.Count);
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
