using UnityEngine;
using UnityEngine.EventSystems;

public class EquippedSlot : Slot
{
    [Header("장착 가능한 아이템 타입")]
    [SerializeField] private ArmorType type;
    [SerializeField] private bool isFirst;

    public override bool SetSlot(int _itemId, int _itemCount)
    {
        var item = ItemManager.Instance.itemDB[_itemId];

        if (item is ArmorInfo isArmor)
        {
            if (isArmor.armorType == type)
            {
                ItemManager.Instance.SetItemSlot(itemId, isFirst);

                itemId = _itemId;
                count = _itemCount;

                icon.color = Color.white;
                icon.sprite = item.icon;

                if (_itemCount > 1) countText.text = count.ToString();
                else countText.text = "";

                return true;
            }
        }

        return false;
    }

    public override void SetSlot(int _itemCount)
    {
        count = _itemCount;

        if (_itemCount > 1)
        {
            countText.text = _itemCount.ToString();
        }

        else
        {
            ItemManager.Instance.SetItemSlot(0, isFirst);

            itemId = 0;
            countText.text = "";
            icon.color = Color.clear;
        }
    }

    protected override bool CheckItem(Slot _dragSlot)
    {
        //주무기에서 왔다면 방어구가 아님
        if (_dragSlot is HandSlot)
        {
            return false;
        }

        else
        {
            //드래그한 장비가 방어구가 아니라면 장착 불가능
            var item = ItemManager.Instance.itemDB[_dragSlot.itemId];
            if (item.itemType != ItemType.Armor) return false;
        }

        return true;
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        var drag = UiManager.instance.status.drag;
        var dragSlot = drag.slot;

        //드래그 중이 아닐 경우
        if (!drag.isClick)
        {
            //마우스만 움직이고 있을 경우 / 아이템이 존재할 경우에만
            if (drag.selectItemId == 0 && itemId != 0)
            {
                drag.SetSlot(pos, this);
            }

            //드래그 중 끝났을 경우
            else if (drag.selectItemId != 0)
            {
                //현재 슬롯에 아이템이 존재할 경우
                if (itemId != 0)
                {
                    if (CheckItem(dragSlot))
                    {
                        var tempItemId = dragSlot.itemId;
                        var tempItemCount = dragSlot.count;

                        //교환 성공시
                        if (dragSlot.SetSlot(itemId, count))
                        {
                            this.SetSlot(tempItemId, tempItemCount);
                        }
                    }
                }

                //현재 슬롯에 아무것도 없을 경우
                else
                {
                    if (this.SetSlot(dragSlot.itemId, dragSlot.count))
                    {
                        dragSlot.SetSlot(0);
                    }
                }

                drag.SetSlot(pos, this);
                drag.EndChangeSlot();
            }
        }
    }
}
