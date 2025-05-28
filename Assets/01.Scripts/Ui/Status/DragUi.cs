using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragUi : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler
{
    public RectTransform pos { get => fieldPos; }
    [SerializeField] private RectTransform fieldPos;
    [SerializeField] private Image icon;

    public bool isClick { get; private set; }
    public int selectItemId { get; private set; }
    public ISlot slot { get; private set; }

    private void Reset()
    {
        if (this.TryGetComponent<Image>(out var target)) icon = target;
        else DebugHelper.ShowBugWindow($"{this.name}�� Image������Ʈ�� �������� ����");

        if (this.TryGetComponent<RectTransform>(out var rect)) fieldPos = rect;
        else DebugHelper.ShowBugWindow($"{this.name}�� RectTransform������Ʈ�� �������� ����");
    }

    /// <summary>
    /// �巹�� ��ġ ����
    /// </summary>
    /// <param name="_slot"></param>
    public void SetSlot(RectTransform _pos, ISlot _slot)
    {
        //���Կ� ��ġ�� �ش� �������� �̵� = �巡�׸� ����
        slot = _slot;
        fieldPos.sizeDelta = _pos.rect.size;
        this.transform.position = _pos.transform.position;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //��ġ�� ���Կ� �������� ������ ��쿡��
        var slotItemId = slot.itemId;

        if (!isClick && slotItemId != 0)
        {
            UiManager.instance.touch.SetTouch(fieldPos, true);
            UiManager.instance.status.itemInfo.SetActive(fieldPos.position, slotItemId);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //�巡�� ���۵�
        isClick = true;
        selectItemId = slot.itemId;
        UiManager.instance.status.itemInfo.SetOff();

        icon.sprite = ItemManager.Instance.itemDB[selectItemId].icon;
        icon.color = Color.white;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //���콺 ��ġ�� ����
        this.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //�巡�� ����
        isClick = false;
        icon.color = Color.clear;
        this.transform.position = Vector3.up * 5000;
    }

    /// <summary>
    /// �巡�� â ����ġ
    /// </summary>
    public void OnEndDrag()
    {
        //�巡�� ����
        isClick = false;
        this.transform.position = Vector3.up * 5000;
    }

    public void EndChangeSlot()
    {
        //���õ� ������ ���� ���� �������� ����
        selectItemId = 0;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //���콺�� ������ �ۿ� �����ٴ°� ������ ����ٴ� �ǹ̷� ����
        UiManager.instance.status.itemInfo.SetOff();
        UiManager.instance.touch.SetTouch(false);
    }
}
