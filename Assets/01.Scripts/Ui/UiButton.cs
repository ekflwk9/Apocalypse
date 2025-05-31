using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class UiButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] protected GameObject touch;

    protected virtual void Reset()
    {
        touch = this.TryFindChild(nameof(touch)).gameObject;
        if (touch.activeSelf) touch.SetActive(false);
    }

    protected void OnDisable()
    {
        touch.SetActive(false);
    }

    public abstract void OnPointerClick(PointerEventData eventData);

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (UiManager.instance.fade.activeSelf) return;
        touch.gameObject.SetActive(true);
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        if (!touch.activeSelf) return;
        touch.gameObject.SetActive(false);
    }
}
