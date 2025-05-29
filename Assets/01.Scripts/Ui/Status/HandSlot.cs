using UnityEngine;
using UnityEngine.EventSystems;

public class HandSlot : Slot
{
    public override bool SetSlot(int _itemId, int _itemCount)
    {
        count = _itemCount;

        if (_itemId != 0 && _itemCount != 0)
        {
            var item = ItemManager.Instance.itemDB[_itemId];
            if (ItemType.Armor == item.itemType) return false;

            //***********************아이템 등록...

            itemId = _itemId;
            icon.color = Color.white;
            icon.sprite = item.icon;

            if (_itemCount > 1) countText.text = count.ToString();
        }

        else
        {
            countText.text = "";
            icon.color = Color.clear;
        }

        return true;
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
            itemId = 0;
            countText.text = "";
            icon.color = Color.clear;

            //주무기 검사 후 삭제
            //ItemManager.Instance.Inventory.RemoveItem(itemId);
        }
    }

    protected override bool CheckItem(Slot _dragSlot)
    {
        //드래그한 장비가 방어구 슬롯에서 왔다면 장착 불가능
        if (_dragSlot is EquippedSlot) return false;
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
                var itemData = ItemManager.Instance.itemDB[drag.selectItemId];

                //아머 타입으로 교체 시도를 하지 않고 있을 경우에만
                if (itemData.itemType != ItemType.Armor)
                {
                    //중복 아이템일 경우
                    if (itemData.itemId == itemId)
                    {
                        //중복 획득 가능 여부, 최대치, 동일 슬롯인지 검사
                        if (itemData.canStack && count + dragSlot.count <= itemData.maxStack)
                        {
                            //참조 주소가 같지 않을 경우에만
                            if (!ReferenceEquals(this, dragSlot))
                            {
                                this.SetSlot(count + dragSlot.count);
                                dragSlot.SetSlot(0);
                            }
                        }
                    }

                    //현재 슬롯에 아이템이 존재할 경우
                    else if (itemId != 0)
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
                }

                drag.SetSlot(pos, this);
                drag.EndChangeSlot();
            }
        }
    }
}
