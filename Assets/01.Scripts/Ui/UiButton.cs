using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class UiButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] protected Image icon;
    [SerializeField] protected GameObject touch;
    [SerializeField] protected TMP_Text info;

    protected virtual void Reset()
    {
        var childCount = this.transform.childCount;

        if (childCount == 0)
        {
            DebugHelper.ShowBugWindow($"{this.name}에 자식 오브젝트이 존재하지 않음");
            return;
        }

        else if (touch == null || icon == null)
        {
            touch = Helper.FindChild(this.transform, nameof(touch)).gameObject;
            icon = Helper.FindChild(this.transform, nameof(icon)).GetComponent<Image>();
            info = Helper.FindChild(this.transform, nameof(info)).GetComponent<TMP_Text>();

            info.text = "";
            if (touch.activeSelf) touch.SetActive(false);
        }
    }

    public abstract void OnPointerClick(PointerEventData eventData);

    public void OnPointerEnter(PointerEventData eventData)
    {
        touch.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (touch.activeSelf) touch.gameObject.SetActive(false);
    }
}
