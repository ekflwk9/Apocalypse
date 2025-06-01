using UnityEngine.EventSystems;

public class SettingButton : UiButton
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        if (!UiManager.instance.fade.activeSelf)
        {
            SoundManager.Play("UI_Click");
            UiManager.instance.fade.OnFade();
            UiManager.instance.menu.menuWindow.SetActive(true);
        }
    }
}
