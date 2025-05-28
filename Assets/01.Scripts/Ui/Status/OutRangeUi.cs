using UnityEngine;
using UnityEngine.EventSystems;

public class OutRangeUi : MonoBehaviour, IPointerEnterHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        var drag = UiManager.instance.status.drag;
        drag.EndChangeSlot();

        //버리시겠습니까 표시
    }
}
