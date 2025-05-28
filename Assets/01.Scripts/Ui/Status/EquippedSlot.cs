using GameItem;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquippedSlot : MonoBehaviour, IPointerEnterHandler, ISlot
{
    [Header("장착 가능한 아이템 타입")]
    [SerializeField] private ArmorType type;
    public int itemId { get; set; }

    [Space(10f)]
    [SerializeField] private Image icon;
    [SerializeField] private RectTransform pos;

    private void Reset()
    {
        if (this.TryGetComponent<Image>(out var isIcon)) icon = isIcon;
        else DebugHelper.Log($"{this.name}에 Image가 존재하지 않음");

        if (this.TryGetComponent<RectTransform>(out var isPos)) pos = isPos;
        else DebugHelper.Log($"{this.name}에 RectTransform가 존재하지 않음");
    }

    public bool SetItem(int _itemId)
    {
        return false;
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
                //아이템이 존재할 경우 맞교환
                var item = itemId != 0 ? itemId : 0;

                if (drag.slot.SetItem(item))
                {
                    SetItem(dragItemId);
                    drag.EndChangeSlot();

                    drag.SetSlot(pos, this);
                }
            }
        }
    }
}
