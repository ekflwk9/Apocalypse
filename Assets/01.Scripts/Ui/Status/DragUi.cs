using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragUi : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public int itemId { get; private set; }
    [SerializeField] private Image icon;

    private void Reset()
    {
        if (this.TryGetComponent<Image>(out var target)) icon = target;
        else DebugHelper.ShowBugWindow($"{this.name}에 Image컴포넌트가 존재하지 않음");

        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// 드래그 창 아이템 정보 설정
    /// </summary>
    /// <param name="_image"></param>
    public void SetItem(int _id)
    {
        itemId = _id;
        //icon.sprite = _image;
        if(_id != 0) this.gameObject.SetActive(true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        this.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        this.gameObject.SetActive(false);
    }
}
