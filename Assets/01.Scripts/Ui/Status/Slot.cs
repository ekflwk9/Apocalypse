using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : UiButton
{
    protected override void Reset()
    {
        base.Reset();
        if (icon != null) icon.color = Color.clear;
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {

        }

        else if (eventData.button == PointerEventData.InputButton.Left)
        {

        }
    }
}
