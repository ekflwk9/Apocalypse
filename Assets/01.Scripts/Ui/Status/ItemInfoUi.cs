using TMPro;
using UnityEngine;

public class ItemInfoUi : MonoBehaviour
{
    [SerializeField] private TMP_Text itemName;
    [SerializeField] private TMP_Text info;

    private void Reset()
    {
        var namePos = Helper.FindChild(this.transform, nameof(itemName));
        itemName = FindText(namePos);

        var infoPos = Helper.FindChild(this.transform, nameof(info));
        info = FindText(infoPos);
    }

    private TMP_Text FindText(Transform _component)
    {
        TMP_Text findText = null;

        if (_component != null)
        {
            if (!_component.TryGetComponent<TMP_Text>(out findText))
            {
                DebugHelper.ShowBugWindow($"{_component.name}�� TMP_Text�� �������� ����");
            }
        }

        return findText;
    }

    /// <summary>
    /// ������ ����â ���� �� Ȱ��ȭ ����
    /// </summary>
    /// <param name="_itemId"></param>
    /// <param name="_eventData"></param>
    /// <param name="_isActive"></param>
    public void SetActive(Vector3 _pos, int _itemId)
    {
        var item = ItemManager.Instance.itemDB[_itemId];
        itemName.text = item.itemName;
        info.text = item.disciption;

        this.gameObject.SetActive(true);
        this.transform.position = _pos;
    }

    /// <summary>
    /// Ȱ��ȭ ���� ��� ��Ȱ��ȭ
    /// </summary>
    public void SetOff()
    {
        if (this.gameObject.activeSelf) this.gameObject.SetActive(false);
    }
}
