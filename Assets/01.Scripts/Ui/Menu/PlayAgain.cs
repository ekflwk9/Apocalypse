using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PlayAgain : UiButton
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        var menu = UiManager.instance.menu;
        var isActive = menu.menuWindow.activeSelf;

        menu.menuWindow.SetActive(!isActive);

        var inventory = UiManager.instance.status.inventory.gameObject.activeSelf;
        var scene = SceneManager.GetActiveScene().name == "Loby";

        if (!inventory && !scene)
        {
            UiManager.instance.shaderEffect.SetActive(!isActive);
            UiManager.instance.SetActive(!isActive);

            Cursor.lockState = isActive ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !isActive;
        }

        else if (scene)
        {
            UiManager.instance.fade.OnFade();
        }

        SoundManager.Play("UI_Click");
        touch.SetActive(false);
    }
}