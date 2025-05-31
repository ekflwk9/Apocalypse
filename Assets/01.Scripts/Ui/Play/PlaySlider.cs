using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaySlider : MonoBehaviour
{
    [SerializeField] private Image slider;

    private void Reset()
    {
        slider = this.TryGetComponent<Image>();
        if (slider == null) slider = this.TryFindChildComponent<Image>("Slider");
    }

    public void SetSlider(float _value)
    {
        slider.fillAmount = _value;
    }
}
