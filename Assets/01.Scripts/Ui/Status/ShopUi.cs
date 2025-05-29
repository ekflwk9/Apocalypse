using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopUi : MonoBehaviour
{
    [SerializeField] private RectTransform pos;

    private void Reset()
    {
        //if(this.TryGetComponent<RectTransform>(out var target))
    }

    public void SetActive(bool _isActive)
    {
        var status = UiManager.instance.status;
        var inventory = status.inventory;

        if (_isActive) inventory.transform.position = new Vector3(96f, 54f, 0f);
        else inventory.transform.position = inventory.transform.position = new Vector3(this.transform.position.x * -1f, 54f, 0f);

        inventory.SetActive(_isActive);
        this.gameObject.SetActive(_isActive);
        UiManager.instance.shader.SetActive(_isActive);

        if (_isActive)
        {
            status.drag.OnEndDrag();
            status.itemInfo.SetOff();
            UiManager.instance.touch.SetTouch(false);
        }
    }
}
