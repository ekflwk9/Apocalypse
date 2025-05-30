using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicVolume : MonoBehaviour
{
    [SerializeField] private Slider slider;

    private void Reset()
    {
        if (this.TryGetComponent<Slider>(out var target)) slider = target;
        else Debug.Log($"{this.name}에 Slider가 존재하지 않음");
    }

    public void SetVoume()
    {
        SoundManager.BackgroundVolume = slider.value;
    }
}
