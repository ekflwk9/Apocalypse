using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragUi : MonoBehaviour, IDragHandler, IEndDragHandler
{
    [SerializeField] private RectTransform pos;
    [SerializeField] private Image icon;
    public InventorySlot beforeSlot { get; private set; }
    public int itemId { get; private set; }

    private void Reset()
    {
        if (this.TryGetComponent<Image>(out var target)) icon = target;
        else DebugHelper.ShowBugWindow($"{this.name}�� Image������Ʈ�� �������� ����");

        if (this.TryGetComponent<RectTransform>(out var rect)) pos = rect;
        else DebugHelper.ShowBugWindow($"{this.name}�� RectTransform������Ʈ�� �������� ����");
    }

    /// <summary>
    /// �巹�� ��ġ ����
    /// </summary>
    /// <param name="_slot"></param>
    public void SetPos(InventorySlot _slot)
    {
        this.transform.position = _slot.transform.position;
        pos.sizeDelta = _slot.slotPos.rect.size;
    }

    /// <summary>
    /// �巡�� â ������ ���� ����
    /// </summary>
    /// <param name="_image"></param>
    public void SetItem(int _id, InventorySlot _slot)
    {
        itemId = _id;
        beforeSlot = _slot;

        icon.sprite = ItemManager.Instance.itemDB[_id].icon;
        icon.color = Color.white;

        if (itemId == 0) DebugHelper.Log("itemId�� 0�� ���� ����");
    }

    public void OnDrag(PointerEventData eventData)
    {
        this.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        itemId = 0;
        icon.color = Color.clear;
        this.transform.position = Vector3.up * 1000;
    }
}
