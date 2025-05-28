using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HandSlot : MonoBehaviour, IPointerEnterHandler, ISlot
{
    public int itemId { get; private set; }
    public int count { get; private set; }

    [Space(10f)]
    [SerializeField] private Image icon;
    [SerializeField] private RectTransform pos;
    [SerializeField] private TMP_Text countText;

    private void Reset()
    {
        countText = Helper.FindChild(this.transform, nameof(countText)).GetComponent<TMP_Text>();
        if (countText != null) countText.text = "";
        else DebugHelper.ShowBugWindow($"{this.name}에 TMP_Text가 존재하지 않음");

        var iconPos = Helper.FindChild(this.transform, nameof(icon)).GetComponent<Image>();
        if (iconPos.TryGetComponent<Image>(out var isIcon)) icon = isIcon;

        if (icon != null) icon.color = Color.clear;
        else DebugHelper.ShowBugWindow($"{this.name}에 Image가 존재하지 않음");

        if (this.TryGetComponent<RectTransform>(out var target)) pos = target;
        else DebugHelper.ShowBugWindow($"{this.name}에 RectTransform가 존재하지 않음");
    }

    public bool SetSlot(int _itemId, int _itemCount)
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

    public void SetSlot(int _itemCount)
    {
        count = _itemCount;

        if (_itemCount > 1)
        {
            countText.text = _itemCount.ToString();
        }

        else
        {
            //***********************아이템 등록...

            itemId = 0;
            countText.text = "";
            icon.color = Color.clear;
        }
    }

    private bool CheckArmor(ISlot _dragSlot)
    {
        //드래그한 장비가 방어구 슬롯에서 왔다면 장착 불가능
        if (_dragSlot is EquippedSlot) return false;
        return true;
    }

    public void OnPointerEnter(PointerEventData eventData)
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
                                //***********************아이템 등록...
                                this.SetSlot(count + dragSlot.count);
                                dragSlot.SetSlot(0);
                            }
                        }
                    }

                    //현재 슬롯에 아이템이 존재할 경우
                    else if (itemId != 0)
                    {
                        if (CheckArmor(dragSlot))
                        {
                            var tempItemId = dragSlot.itemId;
                            var tempItemCount = dragSlot.count;

                            //교환 성공시
                            if (dragSlot.SetSlot(itemId, count))
                            {
                                //***********************아이템 등록...
                                this.SetSlot(tempItemId, tempItemCount);
                            }
                        }
                    }

                    //현재 슬롯에 아무것도 없을 경우
                    else
                    {
                        //***********************아이템 삭제...

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
