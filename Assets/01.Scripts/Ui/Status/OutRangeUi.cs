using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OutRangeUi : MonoBehaviour, IPointerEnterHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(UiManager.instance.status.drag.itemId != 0)
        {
            //UiManager.instance.status
        }
    }
}
