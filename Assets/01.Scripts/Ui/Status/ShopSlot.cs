using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ShopSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("판매할 아이템 ID")]
    [SerializeField] private int itemId;

    [Space(10f)]
    [SerializeField] private Image icon;
    [SerializeField] private RectTransform pos;
    [SerializeField] private TMP_Text countText;

    private void Reset()
    {
        countText = Helper.FindChild(this.transform, nameof(countText)).GetComponent<TMP_Text>();
        if (countText != null) countText.text = "";
        else DebugHelper.ShowBugWindow($"{this.name}에 TMP_Text가 존재하지 않음");

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

    }

    public void OnPointerExit(PointerEventData eventData)
    {

    }
}
