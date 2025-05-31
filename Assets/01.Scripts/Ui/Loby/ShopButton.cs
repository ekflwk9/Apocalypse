using UnityEngine;
using UnityEngine.EventSystems;

public class ShopButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject touch;
    [SerializeField] private string title;
    [SerializeField] private string shopName;
    [SerializeField] private string description;

    private void Reset()
    {
        touch = this.TryFindChild("touch").gameObject;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!UiManager.instance.fade.activeSelf)
        {
            touch.SetActive(true);
            UiManager.instance.status.itemInfo.SetActive(this.transform.position, title, description);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!UiManager.instance.fade.activeSelf)
        {
            touch.SetActive(false);
            UiManager.instance.status.itemInfo.SetOff();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!UiManager.instance.fade.activeSelf)
        {
            var shop = UiManager.instance.status.shop;

            UiManager.instance.fade.OnFade();
            UiManager.instance.lobyUi.title.SetActive(false);

            shop.SetTItle(shopName);
            shop.SetActive(true);
        }
    }
}
