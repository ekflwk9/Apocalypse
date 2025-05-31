using UnityEngine.EventSystems;

public class SettingButton : UiButton
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        if (!UiManager.instance.fade.activeSelf)
        {
            UiManager.instance.fade.OnFade();
            UiManager.instance.menu.menuWindow.SetActive(true);
        }
    }
}
