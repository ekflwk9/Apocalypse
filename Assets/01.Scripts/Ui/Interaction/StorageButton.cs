using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StorageButton : UiButton
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        SoundManager.Play("UI_Click");

        UiManager.instance.fade.OnFade();
        UiManager.instance.status.inventory.SetActive(true);
        UiManager.instance.status.storage.SetActive(true);
        UiManager.instance.status.equipped.SetActive(true);
        UiManager.instance.interactionUi.SwitchBackButton(false);
    }
}
