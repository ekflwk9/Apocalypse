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

        if (this.TryGetComponent<Image>(out var isIcon)) icon = isIcon;
        else DebugHelper.Log($"{this.name}에 Image가 존재하지 않음");

        if (this.TryGetComponent<RectTransform>(out var isPos)) pos = isPos;
        else DebugHelper.Log($"{this.name}에 RectTransform가 존재하지 않음");
    }

    public bool SetSlot(int _itemId, int _itemCount)
    {
        return false;
    }

    public void SetSlot(int _itemCount)
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        var drag = UiManager.instance.status.drag;

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

                //중복 아이템일 경우
                if (itemData.itemId == itemId)
                {
                    //중복 획득 가능 여부, 최대치, 동일 슬롯인지 검사
                    if (itemData.canStack && count <= itemData.maxStack)
                    {
                        //참조 주소가 같지 않을 경우에만
                        if (!ReferenceEquals(this, drag.slot))
                        {
                            //전 슬롯 초기화 후 현재 슬롯 갯수만 추가
                            this.SetSlot(count + drag.slot.count);
                            drag.slot.SetSlot(0);
                        }
                    }
                }

                //현재 슬롯에 아이템이 존재할 경우
                else if (itemId != 0)
                {
                    var tempItemId = drag.slot.itemId;
                    var tempItemCount = drag.slot.count;

                    //맞교환 성공시
                    if (drag.slot.SetSlot(itemId, count))
                    {
                        this.SetSlot(tempItemId, tempItemCount);
                    }
                }

                //현재 슬롯에 아무것도 없을 경우
                else
                {
                    var dragSlot = drag.slot;
                    this.SetSlot(dragSlot.itemId, dragSlot.count);

                    drag.slot.SetSlot(0);
                }

                drag.SetSlot(pos, this);
                drag.EndChangeSlot();
            }
        }
    }
}
