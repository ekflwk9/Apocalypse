using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquippedSlot : MonoBehaviour, IPointerEnterHandler, ISlot
{
    [Header("장착 가능한 아이템 타입")]
    [SerializeField] private ArmorType type;
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
        var item = ItemManager.Instance.itemDB[_itemId];

        if (item is ArmorInfo isArmor)
        {
            if (isArmor.armorType == type)
            {
                //***********************아이템 등록...

                itemId = _itemId;
                count = _itemCount;

                icon.color = Color.white;
                icon.sprite = item.icon;

                if (_itemCount > 1) countText.text = count.ToString();

                return true;
            }
        }

        return false;
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
            itemId = 0;
            countText.text = "";
            icon.color = Color.clear;
        }
    }

    private bool CheckConsumable(ISlot _dragSlot)
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
                //현재 슬롯에 아이템이 존재할 경우
                if (itemId != 0)
                {
                    if (CheckConsumable(dragSlot))
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
