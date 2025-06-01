using UnityEngine;
using UnityEngine.UI;

public class EffectVolume : MonoBehaviour
{
    private void Start()
    {
        var slider = GetComponent<Slider>();
        slider.value = SoundManager.EffectVolume;
    }
    
    public void SetVolume(float value)
    {
        SoundManager.EffectVolume = value;
    }
}
