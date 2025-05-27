using System.Collections;
using System.Collections.Generic;
using GameItem;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EquippedSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, ISlot
{
    [Header("장착 가능한 아이템 타입")]
    [SerializeField] private ArmorType type;
    private int itemId;

    [Space(10f)]
    [SerializeField] private Image icon;
    [SerializeField] private RectTransform pos;

    private void Reset()
    {
        if (this.TryGetComponent<Image>(out var isIcon)) icon = isIcon;
        else DebugHelper.Log($"{this.name}에 Image가 존재하지 않음");

        if (this.TryGetComponent<RectTransform>(out var isPos)) pos = isPos;
        else DebugHelper.Log($"{this.name}에 RectTransform가 존재하지 않음");
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
