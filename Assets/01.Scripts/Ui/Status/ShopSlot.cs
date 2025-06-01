using UnityEngine;
using UnityEngine.EventSystems;

public class ShopSlot : Slot, IPointerClickHandler, IPointerExitHandler
{
    [SerializeField] private int id;

    private void Start()
    {
        if (id != 0)
        {
            itemId = id;

            var item = ItemManager.Instance.GetItem(itemId);
            icon.sprite = item.icon;
            icon.color = Color.white;
            count = 1;
        }

        countText.text = count > 1 ? count.ToString() : "";
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right && id != 0)
        {
            var gold = Player.Instance.Gold;
            var item = ItemManager.Instance.GetItem(itemId);

            if (gold >= item.cost)
            {
                SoundManager.Play("Buy");
                Player.Instance.SetGold(-item.cost);
                UiManager.instance.interactionUi.gold.UpdateGold();
                UiManager.instance.status.GetItem(itemId);
                ItemManager.Instance.Inventory.Add(itemId);
            }

            else
            {
                UiManager.instance.interactionUi.noMoney.gameObject.SetActive(true);
            }
        }
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        var drag = UiManager.instance.status.drag;
        var dragSlot = drag.slot;

        //드래그 중이 아닐 경우
        if (!drag.isClick)
        {
            //마우스만 움직이고 있을 경우 / 아이템이 존재할 경우에만
            if (drag.selectItemId == 0 && itemId != 0)
            {
                UiManager.instance.touch.SetTouch(pos, true);
                UiManager.instance.status.itemInfo.SetActive(pos.position, itemId);
            }

            //드래그 중 끝났을 경우
            else if (drag.selectItemId != 0)
            {
                var item = ItemManager.Instance.GetItem(dragSlot.itemId);

                SoundManager.Play("Buy");
                Player.Instance.SetGold(item.cost);
                UiManager.instance.interactionUi.gold.UpdateGold();

                dragSlot.SetSlot(0);
                drag.EndChangeSlot();

                if (itemId != 0)
                {
                    UiManager.instance.touch.SetTouch(pos, true);
                    UiManager.instance.status.itemInfo.SetActive(pos.position, itemId);
                }
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (itemId != 0)
        {
            UiManager.instance.touch.SetTouch(false);
            UiManager.instance.status.itemInfo.SetOff();
        }
    }

    protected override bool CheckItem(Slot _dragSlot) => true;
    public override bool SetSlot(int _itemId, int _itemCount) => true;
}
