using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

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
            if(!_component.TryGetComponent<TMP_Text>(out findText))
            {
                DebugHelper.ShowBugWindow($"{_component.name}에 TMP_Text가 존재하지 않음");
            }
        }

        return findText;
    }

    /// <summary>
    /// 아이템 정보창 설정 및 활성화 여부
    /// </summary>
    /// <param name="_itemId"></param>
    /// <param name="_eventData"></param>
    /// <param name="_isActive"></param>
    public void SetActive(int _itemId, Vector3 _pos, bool _isActive)
    {
        //아이템 아이디로 정보 가져오기...

        this.gameObject.SetActive(_isActive);
        this.transform.position = _pos;
    }
}
