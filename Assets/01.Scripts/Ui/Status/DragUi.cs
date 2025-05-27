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
        else DebugHelper.ShowBugWindow($"{this.name}�� Image������Ʈ�� �������� ����");

        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// �巡�� â ������ ���� ����
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
