using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayAgain : UiButton
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        var menu = UiManager.instance.menu;
        var isActive = menu.menuWindow.activeSelf;

        menu.menuWindow.SetActive(!isActive);

        if (!UiManager.instance.status.inventory.gameObject.activeSelf)
        {
            UiManager.instance.shaderEffect.SetActive(!isActive);

            Cursor.lockState = isActive ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !isActive;
        }

        touch.SetActive(false);
    }
}
