using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayAgain : UiButton
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        UiManager.instance.menu.menuWindow.SetActive(false);
        UiManager.instance.shader.SetActive(false);
        touch.SetActive(false);
    }
}
