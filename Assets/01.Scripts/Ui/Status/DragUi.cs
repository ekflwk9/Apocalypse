using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragUi : MonoBehaviour, IDragHandler, IEndDragHandler
{
    [SerializeField] private RectTransform pos;
    [SerializeField] private Image icon;
    public int itemId { get; private set; }
    public ISlot slot { get; private set; }

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
    public void SetPos(RectTransform _pos)
    {
        this.transform.position = _pos.transform.position;
        pos.sizeDelta = _pos.rect.size;
    }

    /// <summary>
    /// �巡�� â ������ ���� ����
    /// </summary>
    /// <param name="_image"></param>
    public void ClickItem(int _id, ISlot _slot)
    {
        itemId = _id;
        slot = _slot;

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
