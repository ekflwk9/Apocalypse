using UnityEngine;
using UnityEngine.EventSystems;

public class FarmingSlot : Slot
{
    public override bool SetSlot(int _itemId, int _itemCount)
    {
        count = _itemCount;

        if (_itemId != 0 && _itemCount != 0)
        {
            var item = ItemManager.Instance.GetItem(_itemId);

            itemId = _itemId;
            icon.color = Color.white;
            icon.sprite = item.icon;

            if (_itemCount > 1) countText.text = count.ToString();
            else countText.text = "";
        }

        else
        {
            countText.text = "";
            icon.color = Color.clear;
        }

        return true;
    }

    protected override bool CheckItem(Slot _dragSlot)
    {
        //주무기 슬롯에서 왔을 경우에만
        if (_dragSlot is HandSlot)
        {
            //현재 슬롯 아이템 타입이 방어구가 아닐 경우에만 교환
            var item = ItemManager.Instance.GetItem(itemId);
            if (item.itemType == ItemType.Armor) return false;
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
                var itemData = ItemManager.Instance.GetItem(drag.selectItemId);

                //중복 아이템일 경우
                if (itemData.itemId == itemId)
                {
                    //중복 획득 가능 여부, 최대치
                    if (itemData.canStack && count + dragSlot.count <= itemData.maxStack)
                    {
                        //참조 주소가 같지 않을 경우에만
                        if (!ReferenceEquals(this, dragSlot))
                        {
                            //전 슬롯 초기화 후 현재 슬롯 갯수만 추가
                            this.SetSlot(count + dragSlot.count);
                            dragSlot.SetSlot(0);
                        }
                    }
                }

                //현재 슬롯에 아이템이 존재할 경우
                else if (itemId != 0)
                {
                    //현재 주무기에서 아머 타입으로 교체 시도를 하고 있는가?
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
