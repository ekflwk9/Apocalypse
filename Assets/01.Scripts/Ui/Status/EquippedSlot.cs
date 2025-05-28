using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquippedSlot : MonoBehaviour, IPointerEnterHandler, ISlot
{
    [Header("���� ������ ������ Ÿ��")]
    [SerializeField] private ArmorType type;
    public int itemId { get; set; }

    [Space(10f)]
    [SerializeField] private Image icon;
    [SerializeField] private RectTransform pos;

    private void Reset()
    {
        if (this.TryGetComponent<Image>(out var isIcon)) icon = isIcon;
        else DebugHelper.Log($"{this.name}�� Image�� �������� ����");

        if (this.TryGetComponent<RectTransform>(out var isPos)) pos = isPos;
        else DebugHelper.Log($"{this.name}�� RectTransform�� �������� ����");
    }

    public bool SetItem(int _itemId)
    {
        return false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        var status = UiManager.instance.status;
        var drag = status.drag;
        var dragItemId = drag.selectItemId;

        //�巡�� ���� �ƴ� ��쿡��
        if (!drag.isClick)
        {
            //���콺�� �����̰� ���� ���
            if (dragItemId == 0)
            {
                //�������� ������ ��쿡��
                if (itemId != 0)
                {
                    drag.SetSlot(pos, this);
                }
            }

            //�巡�� �� ������ ���
            else
            {
                //�������� ������ ��� �±�ȯ
                var item = itemId != 0 ? itemId : 0;

                if (drag.slot.SetItem(item))
                {
                    SetItem(dragItemId);
                    drag.EndChangeSlot();

                    drag.SetSlot(pos, this);
                }
            }
        }
    }
}
