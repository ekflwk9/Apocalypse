using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlaySlot : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text countText;

    private void Reset()
    {
        icon = this.TryFindChildComponent<Image>(nameof(icon));
        countText = this.TryFindChildComponent<TMP_Text>(nameof(countText));
    }

    /// <summary>
    /// 화면에 보이는 슬롯 
    /// </summary>
    /// <param name="_itemId"></param>
    /// <param name="_count"></param>
    public void SetSlotView(int _itemId, int _count)
    {
        var item = ItemManager.Instance.itemDB[_itemId];
        icon.sprite = item.icon;
        icon.color = Color.white;

        if (_count > 1) countText.text = _count.ToString();
        else countText.text = "";
    }

    public void SetSlotView(int _count)
    {
        if (_count > 1) countText.text = _count.ToString();
        else countText.text = "";
    }

    /// <summary>
    /// 매개변수가 없다면 화면에 보이는 슬롯을 지워줌
    /// </summary>
    /// <param name="_count"></param>
    public void SetSlotView()
    {
        countText.text = "";
        icon.color = Color.clear;
    }
}
