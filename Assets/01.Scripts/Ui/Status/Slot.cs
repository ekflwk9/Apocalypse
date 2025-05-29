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
