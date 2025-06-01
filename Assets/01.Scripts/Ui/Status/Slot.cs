using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class Slot : MonoBehaviour, IPointerEnterHandler
{
    public int itemId { get; protected set; }
    public int count { get; protected set; }

    [SerializeField] protected Image icon;
    [SerializeField] protected RectTransform pos;
    [SerializeField] protected TMP_Text countText;

    protected virtual void Reset()
    {
        countText = this.TryFindChildComponent<TMP_Text>(nameof(countText));
        if (countText != null) countText.text = "";

        icon = this.TryFindChildComponent<Image>(nameof(icon));
        if (icon != null) icon.color = Color.clear;

        pos = this.TryGetComponent<RectTransform>();
    }

    public abstract void OnPointerEnter(PointerEventData eventData);

    protected abstract bool CheckItem(Slot _dragSlot);

    public abstract bool SetSlot(int _itemId, int _itemCount);

    public virtual void SetSlot(int _itemCount)
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
}
