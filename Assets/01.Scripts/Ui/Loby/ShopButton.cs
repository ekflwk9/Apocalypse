using UnityEngine;
using UnityEngine.EventSystems;

public class ShopButton : UiButton
{
    [SerializeField] private string title;
    [SerializeField] private string shopName;
    [SerializeField] private string description;

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (UiManager.instance.fade.activeSelf) return;
        touch.gameObject.SetActive(true);
        UiManager.instance.status.itemInfo.SetActive(this.transform.position, title, description);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        if (!touch.activeSelf) return;
        touch.gameObject.SetActive(false);
        UiManager.instance.status.itemInfo.SetOff();
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (!UiManager.instance.fade.activeSelf)
        {
            var shop = UiManager.instance.status.shop;

            UiManager.instance.fade.OnFade();
            UiManager.instance.lobyUi.title.SetActive(false);
            UiManager.instance.interactionUi.SwitchBackButton(false);

            SoundManager.Play("UI_Click");
            shop.SetTItle(shopName);
            shop.SetActive(true);
        }
    }
}
