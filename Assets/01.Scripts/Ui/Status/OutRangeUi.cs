using UnityEngine;
using UnityEngine.EventSystems;

public class OutRangeUi : MonoBehaviour, IPointerEnterHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        var drag = UiManager.instance.status.drag;
        drag.EndChangeSlot();

        //�����ðڽ��ϱ� ǥ��
    }
}
