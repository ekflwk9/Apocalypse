using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class UiButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] protected GameObject touch;
    [SerializeField] protected TMP_Text info;

    protected virtual void Reset()
    {
        var childCount = this.transform.childCount;

        if (childCount == 0)
        {
            DebugHelper.ShowBugWindow($"{this.name}�� �ڽ� ������Ʈ�� �������� ����");
            return;
        }

        touch = Helper.FindChild(this.transform, nameof(touch)).gameObject;

        info = GetComponentInChildren<TMP_Text>();
        if (info == null) DebugHelper.ShowBugWindow($"{this.name} �ڽ� ������Ʈ�� TMP_text�� �������� ����");

        info.text = "";
        if (touch.activeSelf) touch.SetActive(false);
    }

    public abstract void OnPointerClick(PointerEventData eventData);

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        touch.gameObject.SetActive(true);
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        if (touch.activeSelf) touch.gameObject.SetActive(false);
    }
}
