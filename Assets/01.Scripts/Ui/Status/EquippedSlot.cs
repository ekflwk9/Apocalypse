using System.Collections;
using System.Collections.Generic;
using GameItem;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EquippedSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, ISlot
{
    [Header("���� ������ ������ Ÿ��")]
    [SerializeField] private ArmorType type;
    private int itemId;

    [Space(10f)]
    [SerializeField] private Image icon;
    [SerializeField] private RectTransform pos;

    private void Reset()
    {
        if (this.TryGetComponent<Image>(out var isIcon)) icon = isIcon;
        else DebugHelper.Log($"{this.name}�� Image�� �������� ����");

        if (this.TryGetComponent<RectTransform>(out var isPos)) pos = isPos;
        else DebugHelper.Log($"{this.name}�� RectTransform�� �������� ����");
    }

    public void SetItem(int _itemId)
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {

    }

    public void OnPointerExit(PointerEventData eventData)
    {

    }

}
