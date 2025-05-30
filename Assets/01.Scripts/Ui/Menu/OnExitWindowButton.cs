using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnExitWindowButton : UiButton
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        UiManager.instance.menu.exitWindow.SetActive(true);
    }
}
