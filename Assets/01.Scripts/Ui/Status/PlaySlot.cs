using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlaySlot : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text countText;

    private void Reset()
    {
        var iconPos = Helper.FindChild(this.transform, nameof(icon));
        if (iconPos.TryGetComponent<Image>(out var isIcon)) icon = isIcon;
        else DebugHelper.ShowBugWindow($"{this.name}에 Image가 존재하지 않음");

        var countPos = Helper.FindChild(this.transform, nameof(countText));
        if (countPos.TryGetComponent<TMP_Text>(out var isCount)) countText = isCount;
        else DebugHelper.ShowBugWindow($"{this.name}에 TMP_Text가 존재하지 않음");
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

        if (_count > 1)
        {
            icon.color = Color.white;
            countText.text = _count.ToString();
        }
        else
        {
            icon.color = Color.white;
            countText.text = _count.ToString();
        }
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
