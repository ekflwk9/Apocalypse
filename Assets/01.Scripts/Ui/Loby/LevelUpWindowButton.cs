using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelUpWindowButton : UiButton
{
    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (UiManager.instance.fade.activeSelf) return;
        touch.SetActive(true);
        UiManager.instance.status.itemInfo.SetActive(this.transform.position, "\"시민 위원회\"",
        "여의도를 지배하는 권력자들을 위한 특별한 공간\r시민 등급을 올리려면 반드시 가야하는 곳이다.");
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        if (!touch.activeSelf) return;
        touch.gameObject.SetActive(false);
        UiManager.instance.status.itemInfo.SetOff();
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        UiManager.instance.fade.OnFade();
        UiManager.instance.lobyUi.levelUpWindow.SetActive(true);
        UiManager.instance.interactionUi.SwitchBackButton(false);
    }
}
