using UnityEngine;
using UnityEngine.UI;

public class MusicVolume : MonoBehaviour
{
    private void Start()
    {
        var slider = GetComponent<Slider>();
        slider.value = SoundManager.BackgroundVolume;
    }
    
    public void SetVolume(float value)
    {
        SoundManager.BackgroundVolume = value;
    }
}
