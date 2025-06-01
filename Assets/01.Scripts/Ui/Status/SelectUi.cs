using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectUi : MonoBehaviour, 
IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler
{
    public RectTransform pos { get => fieldPos; }
    [SerializeField] private RectTransform fieldPos;
    [SerializeField] private Image icon;

    public bool isClick { get; private set; }
    public int selectItemId { get; private set; }
    public Slot slot { get; private set; }

    private void Reset()
    {
        if (this.TryGetComponent<Image>(out var target)) icon = target;
        else DebugHelper.ShowBugWindow($"{this.name}에 Image컴포넌트가 존재하지 않음");

        if (this.TryGetComponent<RectTransform>(out var rect)) fieldPos = rect;
        else DebugHelper.ShowBugWindow($"{this.name}에 RectTransform컴포넌트가 존재하지 않음");
    }

    /// <summary>
    /// 드레그 위치 설정
    /// </summary>
    /// <param name="_slot"></param>
    public void SetSlot(RectTransform _pos, Slot _slot)
    {
        //슬롯에 터치시 해당 슬롯으로 이동 = 드래그를 위함
        slot = _slot;
        fieldPos = _pos;
        this.transform.position = _pos.transform.position;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //터치한 슬롯에 아이템이 존재할 경우에만
        var slotItemId = slot.itemId;

        if (!isClick && slotItemId != 0)
        {
            UiManager.instance.touch.SetTouch(fieldPos, true);
            UiManager.instance.status.itemInfo.SetActive(fieldPos.position, slotItemId);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //드래그 시작됨
        isClick = true;
        selectItemId = slot.itemId;
        SoundManager.Play("UI_Click");

        UiManager.instance.status.itemInfo.SetOff();
        icon.sprite = ItemManager.Instance.GetItem(selectItemId).icon;
        icon.color = Color.white;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //마우스 위치를 추적
        this.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //드래그 끝남
        isClick = false;
        icon.color = Color.clear;
        this.transform.position = Vector3.up * 5000;
    }

    /// <summary>
    /// 드래그 창 원위치
    /// </summary>
    public void OnEndDrag()
    {
        //드래그 끝남
        isClick = false;
        this.transform.position = Vector3.up * 5000;
    }

    public void EndChangeSlot()
    {
        //선택된 아이템 존재 하지 않음으로 변경
        selectItemId = 0;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //마우스가 완전히 밖에 나갔다는건 범위를 벗어났다는 의미로 간주
        UiManager.instance.status.itemInfo.SetOff();
        UiManager.instance.touch.SetTouch(false);
    }
}
