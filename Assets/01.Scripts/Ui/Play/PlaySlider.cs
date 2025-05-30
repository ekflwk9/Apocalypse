using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaySlider : MonoBehaviour
{
    [SerializeField] private Image slider;

    private void Reset()
    {
        if (this.TryGetComponent<Image>(out var target)) slider = target;
        else DebugHelper.Log($"{this.name}에 Image컴포넌트가 존재하지 않음");
    }

    public void SetSlider(float _value)
    {
        slider.fillAmount = _value;
    }
}
