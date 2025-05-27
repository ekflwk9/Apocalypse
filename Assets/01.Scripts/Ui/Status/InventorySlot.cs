using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public RectTransform slotPos { get => pos; }
    [SerializeField] private RectTransform pos;
    [SerializeField] private Image icon;

    public int itemId { get; private set; } = 0;
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
    public void SetItem(int _itemId)
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
            if (count == 0) icon.color = Color.clear;
        }
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

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UiManager.instance.touch.SetTouch(false);
        UiManager.instance.status.itemInfo.SetOff();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (itemId != 0)
            {
                UiManager.instance.touch.SetTouch(false);

                var status = UiManager.instance.status;
                status.drag.SetItem(itemId, this);
                status.itemInfo.SetOff();
            }
        }

        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            //������ ����..
        }
    }
}
