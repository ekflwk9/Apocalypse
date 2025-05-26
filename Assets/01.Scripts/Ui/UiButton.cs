using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class UiButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] protected Image icon;
    [SerializeField] protected GameObject touch;
    //[SerializeField] protected TMP_Text number;

    protected virtual void Reset()
    {
        var childCount = this.transform.childCount;

        if (childCount == 0)
        {
            Debug.Log($"{this.name}�� �ڽ� ������Ʈ�� �������� ����");
            return;
        }

        else if (touch == null || icon == null)
        {
            touch = FindChild(this.transform, nameof(touch)).gameObject;
            icon = FindChild(this.transform, nameof(icon)).GetComponent<Image>();
            //number = FindChild(this.transform, nameof(touch)).GetComponent<TMP_Text>();

            if(touch.activeSelf) touch.SetActive(false);
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



    //Helper
    private Transform FindChild(Transform _parent, string _findName)
    {
        var child = TryFindChild(_parent, _findName);
        if (child == null) Debug.Log($"{_parent.name}�� {_findName}��� �ڽ� ������Ʈ�� �������� ����");

        return child;
    }

    private Transform TryFindChild(Transform _parent, string _findName)
    {
        Transform findCHild = null;

        for (int i = 0; i < _parent.childCount; i++)
        {
            var child = _parent.GetChild(i);
            findCHild = child.name == _findName ? child : TryFindChild(child, _findName);
            if (findCHild != null) break;
        }

        return findCHild;
    }
}
