using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PlayButton : UiButton
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        if (UiManager.instance.fade.activeSelf) return;

        Player.Instance.OnStart();
        SoundManager.Play("UI_Click");
        UiManager.instance.fade.OnFade(ChangeScene, 0.5f);


        touch.SetActive(false);
    }

    private void ChangeScene()
    {
        SoundManager.Play("OutSide_City", SoundType.Background);
        SceneManager.LoadScene("Play");
        UiManager.instance.fade.OnFade(0.5f);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        UiManager.instance.SetActive(false);
        UiManager.instance.lobyUi.gameObject.SetActive(false);
        UiManager.instance.interactionUi.gameObject.SetActive(false);

        var isMenu = UiManager.instance.menu.menuWindow;
        if (isMenu.activeSelf) UiManager.instance.menu.menuWindow.SetActive(false);
    }
}
