using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] protected Image icon;
    public int itemId { get; private set; } = 0;
    public int count { get; private set; } = 0;

    protected void Reset()
    {
        icon = Helper.FindChild(this.transform, nameof(icon)).GetComponent<Image>();
        if (icon != null) icon.color = Color.clear;
    }

    /// <summary>
    /// 해당 슬롯에 아이템 설정
    /// </summary>
    /// <param name="_itemId"></param>
    public void SetItem(int _itemId)
    {
        itemId = _itemId;

        if (itemId != 0)
        {
            icon.color = Color.white;
            //icon.sprite = GeItem().sprite //아이템 이미지 설정..
        }
    }

    /// <summary>
    /// 해당 슬롯에 아이템 갯수 설정
    /// </summary>
    /// <param name="_count"></param>
    public void SetItemCount(int _count)
    {
        count = _count;

        if (count == 0) icon.color = Color.clear;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //if (itemId != 0)
        //{
        //    touch.SetActive(true);
        //    UiManager.instance.status.itemInfo.SetActive(0, eventData, true);
        //}

        //test
        UiManager.instance.status.itemInfo.SetActive(0, this.transform.position, true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UiManager.instance.status.itemInfo.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {

        }

        else if (eventData.button == PointerEventData.InputButton.Left)
        {

        }
    }
}
