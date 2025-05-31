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
        var item = ItemManager.Instance.GetItem(_itemId);
        icon.sprite = item.icon;
        icon.color = Color.white;

        if (_count > 1) countText.text = _count.ToString();
        else countText.text = "";
    }

    public void SetSlotView(int _count)
    {
        if (_count > 1)
        {
            countText.text = _count.ToString();
        }

        else if (_count > 0)
        {
            countText.text = "";
        }

        else
        {
            countText.text = "";
            icon.color = Color.clear;
        }
    }
}
