using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("�Ǹ��� ������ ID")]
    [SerializeField] private int itemId;

    [Space(10f)]
    [SerializeField] private Image icon;
    [SerializeField] private RectTransform pos;

    private void Reset()
    {
        if (this.TryGetComponent<Image>(out var isIcon)) icon = isIcon;
        else DebugHelper.ShowBugWindow($"{this.name}�� Image�� �������� ����");

        if (this.TryGetComponent<RectTransform>(out var isPos)) pos = isPos;
        else DebugHelper.ShowBugWindow($"{this.name}�� RectTransform�� �������� ����");
    }

    private void Start()
    {      
        var item = ItemManager.Instance.itemDB[itemId];
        icon.sprite = item.icon;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            //��� �˻�
            UiManager.instance.status.GetItem(itemId);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UiManager.instance.touch.SetTouch(pos, true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UiManager.instance.touch.SetTouch(false);
    }
}
