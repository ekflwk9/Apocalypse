using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("판매할 아이템 ID")]
    [SerializeField] private int itemId;

    [Space(10f)]
    [SerializeField] private Image icon;
    [SerializeField] private RectTransform pos;

    private void Reset()
    {
        if (this.TryGetComponent<Image>(out var isIcon)) icon = isIcon;
        else DebugHelper.ShowBugWindow($"{this.name}에 Image가 존재하지 않음");

        if (this.TryGetComponent<RectTransform>(out var isPos)) pos = isPos;
        else DebugHelper.ShowBugWindow($"{this.name}에 RectTransform가 존재하지 않음");
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
            //골드 검사
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
