using UnityEngine;
using UnityEngine.EventSystems;

public class ShopSlot : Slot, IPointerClickHandler
{
    [SerializeField] private int id;

    private void Start()
    {
        itemId = id;

        var item = ItemManager.Instance.itemDB[itemId];
        icon.sprite = item.icon;

        count = 1;
        countText.text = count > 1 ? count.ToString() : "";
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

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {

            //*******************골드 검사 / 무게 검사
            UiManager.instance.status.GetItem(itemId);
        }
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {

    }


    protected override bool CheckItem(Slot _dragSlot) => true;
    public override bool SetSlot(int _itemId, int _itemCount) => true;
}
