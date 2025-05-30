using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaySlider : MonoBehaviour
{
    [SerializeField] private Image slider;

    private void Reset()
    {
        var pos = Helper.FindChild(this.transform, "Slider");

        if (pos == null )
        {
            if(this.TryGetComponent<Image>(out var target)) slider = target;
        }
        else
        {
            if(pos.TryGetComponent<Image>(out var target)) slider = target;
        }
    }

    public void SetSlider(float _value)
    {
        slider.fillAmount = _value;
    }
}
