using UnityEngine.EventSystems;

public class BackButton : UiButton
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        UiManager.instance.fade.OnFade();
        var levelWindow = UiManager.instance.lobyUi.levelUpWindow;

        if (levelWindow.activeSelf)
        {
            levelWindow.SetActive(false);
        }

        else if (UiManager.instance.status.storage.activeSelf)
        {
            UiManager.instance.status.inventory.SetActive(false);
            UiManager.instance.status.storage.SetActive(false);
            UiManager.instance.status.equipped.SetActive(false);
        }

        else
        {
            var status = UiManager.instance.status;
            var firstShop = status.firstShop;
            var sceondShop = status.secondShop;

            if (firstShop.gameObject.activeSelf) firstShop.SetActive(false);
            else sceondShop.SetActive(false);
        }

        SoundManager.Play("UI_Click");
        UiManager.instance.interactionUi.SwitchBackButton(true);
    }
}
