using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, ISlot
{
    public RectTransform slotPos { get => pos; }
    [SerializeField] private RectTransform pos;
    [SerializeField] private Image icon;

    public int itemId { get; set; }
    public int count { get; private set; } = 0;

    protected void Reset()
    {
        icon = Helper.FindChild(this.transform, nameof(icon)).GetComponent<Image>();
        if (icon != null) icon.color = Color.clear;

        if (this.TryGetComponent<RectTransform>(out var target)) pos = target;
        else DebugHelper.ShowBugWindow($"{this.name}에 RectTransform가 존재하지 않음");
    }

    /// <summary>
    /// 해당 슬롯에 아이템 설정
    /// </summary>
    /// <param name="_itemId"></param>
    public bool SetItem(int _itemId)
    {
        itemId = _itemId;

        if (itemId != 0)
        {
            var item = ItemManager.Instance.itemDB[itemId];

            icon.color = Color.white;
            icon.sprite = item.icon;
        }

        else
        {
            count = 0;
            icon.color = Color.clear;
        }

        return true;
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
        var status = UiManager.instance.status;
        var drag = status.drag;
        var dragItemId = drag.selectItemId;

        //드래그 중이 아닐 경우에만
        if (!drag.isClick)
        {
            //마우스만 움직이고 있을 경우
            if (dragItemId == 0)
            {
                //아이템이 존재할 경우에만
                if (itemId != 0)
                {
                    drag.SetSlot(pos, this);
                }
            }

            //드래그 중 끝났을 경우
            else
            {
                var item = itemId != 0 ? itemId : 0;

                var itemData = ItemManager.Instance.itemDB[item];

                //같은 아이템일 경우
                if (drag.selectItemId == itemId)
                {
                    //중복 획득 가능 여부, 최대치, 동일 슬롯인지 검사
                    if (itemData.canStack && count <= itemData.maxStack && pos != drag.pos)
                    {

                    }
                }

                //아이템이 존재할 경우 맞교환
                //var item = itemId != 0 ? itemId : 0;

                //if (drag.slot.SetItem(item))
                //{
                //    SetItem(dragItemId);
                //    drag.EndChangeSlot();

                //    drag.SetSlot(pos, this);
                //}
            }
        }
    }
}
