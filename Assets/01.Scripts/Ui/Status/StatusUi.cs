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
    /// 켜져있을 경우 꺼짐 / 꺼져있을 경우 켜짐
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
            DebugHelper.Log($"{_uiType}이라는 값이 존재하지 않음");
        }
    }

   
    /// <summary>
    /// Ui 활성화 상태를 직접 지정함
    /// </summary>
    /// <param name="_uiType"></param>
    /// <param name="_isActive"></param>
    public void SetWindow(UiCode _uiType, bool _isActive)
    {
        if (window.ContainsKey(_uiType)) window[_uiType].SetActive(_isActive);
        else DebugHelper.Log($"{_uiType}이라는 값이 존재하지 않음");
    }
}
