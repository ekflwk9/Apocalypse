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
        else DebugHelper.ShowBugWindow($"{this.name}�� RectTransform�� �������� ����");
    }

    /// <summary>
    /// �ش� ���Կ� ������ ����
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
    /// �ش� ���Կ� ������ ���� ����
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

        //�巡�� ���� �ƴ� ��쿡��
        if (!drag.isClick)
        {
            //���콺�� �����̰� ���� ���
            if (dragItemId == 0)
            {
                //�������� ������ ��쿡��
                if (itemId != 0)
                {
                    drag.SetSlot(pos, this);
                }
            }

            //�巡�� �� ������ ���
            else
            {
                var item = itemId != 0 ? itemId : 0;

                var itemData = ItemManager.Instance.itemDB[item];

                //���� �������� ���
                if (drag.selectItemId == itemId)
                {
                    //�ߺ� ȹ�� ���� ����, �ִ�ġ, ���� �������� �˻�
                    if (itemData.canStack && count <= itemData.maxStack && pos != drag.pos)
                    {

                    }
                }

                //�������� ������ ��� �±�ȯ
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
