using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ShopUi : MonoBehaviour
{
    [SerializeField] private TMP_Text title;

    private void Reset()
    {
        title = this.TryFindChildComponent<TMP_Text>("TitleText");
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
        if(!_isActive) inventory.transform.position = new Vector3(960, 447, 0);
        else inventory.transform.position = new Vector3(1230, 447, 0);

        this.gameObject.SetActive(_isActive);
        inventory.gameObject.SetActive(_isActive);

        if (_isActive)
        {
            status.drag.OnEndDrag();
            status.itemInfo.SetOff();
            UiManager.instance.touch.SetTouch(false);
        }
    }
}
