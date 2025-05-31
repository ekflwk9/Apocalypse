using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelUp : UiButton
{
    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        UiManager.instance.status.itemInfo.SetActive(this.transform.position, "\"시민 위원회\"",
        "여의도를 지배하는 권력자들을 위한 특별한 공간\r\n요즘 세상에 이런 곳이 있을까 싶을 정도로 잘 꾸며진 공간이다.\r\n\r\n시민 등급을 올리려면 반드시 가야하는 곳이다.");
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        UiManager.instance.status.itemInfo.SetOff();
    }

    public override void OnPointerClick(PointerEventData eventData)
    {

    }
}
