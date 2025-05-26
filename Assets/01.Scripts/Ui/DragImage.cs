using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragImage : MonoBehaviour, IDragHandler, IEndDragHandler
{
    private Image icon;

    public void ShowIcon(Sprite _image)
    {
        icon.sprite = _image;
        this.gameObject.SetActive(true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(gameObject.activeSelf)
        {

        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {

    }
}
