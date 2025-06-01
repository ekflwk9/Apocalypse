using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NoExitButton : UiButton
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        UiManager.instance.menu.exitWindow.SetActive(false);
        SoundManager.Play("UI_Click");
        touch.SetActive(false);
    }
}
