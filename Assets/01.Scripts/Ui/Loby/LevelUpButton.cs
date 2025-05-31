using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelUpButton : UiButton
{
    private void Start()
    {
        UiManager.instance.lobyUi.UpdateLockWindow();
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        var loby = UiManager.instance.lobyUi;
        var cost = loby.card.GetCost();

        if (cost <= Player.Instance.Gold)
        {
            //사운드 추가
            Player.Instance.SetGold(-cost);
            Player.Instance.SetLevel(1);

            loby.UpdateLockWindow();
            UiManager.instance.lobyUi.card.UpdateCard();
            UiManager.instance.interactionUi.gold.UpdateGold();
        }
    }
}
