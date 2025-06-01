using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

        else if(UiManager.instance.status.storage.activeSelf)
        {
            UiManager.instance.status.inventory.SetActive(false);
            UiManager.instance.status.storage.SetActive(false);
            UiManager.instance.status.equipped.SetActive(false);
        }

        else
        {
            UiManager.instance.status.shop.SetActive(false);
        }

        SoundManager.Play("UI_Click");
        UiManager.instance.interactionUi.SwitchBackButton(true);
    }
}
