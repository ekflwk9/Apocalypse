using TMPro;
using UnityEngine;

public class ShopUi : MonoBehaviour
{
    [SerializeField] private TMP_Text title;

    private void Reset()
    {
        var titlePos = Helper.FindChild(this.transform, "TitleText");

        if (titlePos.TryGetComponent<TMP_Text>(out var target)) title = target;
        else DebugHelper.ShowBugWindow($"{this.name}에 TMP_Text는 존재하지 않음");
    }

    public void SetTItle(string _title)
    {
        title.text = _title;
    }

    public void SetActive(bool _isActive)
    {
        var status = UiManager.instance.status;
        var inventory = status.inventory;

        //인벤토리 위치 조정
        if(!_isActive) inventory.transform.position = new Vector3(960, 540, 0);
        else inventory.transform.position = new Vector3(1230, 540, 0);

        this.gameObject.SetActive(_isActive);
        inventory.gameObject.SetActive(_isActive);
        UiManager.instance.shader.SetActive(_isActive);

        if (_isActive)
        {
            status.drag.OnEndDrag();
            status.itemInfo.SetOff();
            UiManager.instance.touch.SetTouch(false);
        }
    }
}
