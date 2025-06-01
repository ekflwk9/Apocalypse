using TMPro;
using UnityEngine;

public class ItemInfoUi : MonoBehaviour
{
    [SerializeField] private TMP_Text itemName;
    [SerializeField] private TMP_Text info;

    private void Reset()
    {
        itemName = this.TryFindChildComponent<TMP_Text>(nameof(itemName));
        info = this.TryFindChildComponent<TMP_Text>(nameof(info));
    }

    /// <summary>
    /// 아이템 정보창 설정 및 활성화 여부
    /// </summary>
    /// <param name="_itemId"></param>
    /// <param name="_eventData"></param>
    /// <param name="_isActive"></param>
    public void SetActive(Vector3 _pos, int _itemId)
    {
        var item = ItemManager.Instance.GetItem(_itemId);
        itemName.text = item.itemName;
        info.text = item.disciption;

        this.gameObject.SetActive(true);
        this.transform.position = _pos;
    }

    public void SetActive(Vector3 _pos, string _name, string _text)
    {
        itemName.text = _name;
        info.text = _text;

        this.gameObject.SetActive(true);
        this.transform.position = _pos;
    }

    /// <summary>
    /// 활성화 중일 경우 비활성화
    /// </summary>
    public void SetOff()
    {
        if (this.gameObject.activeSelf) this.gameObject.SetActive(false);
    }
}
