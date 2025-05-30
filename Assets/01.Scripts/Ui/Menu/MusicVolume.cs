using UnityEngine;

public class MusicVolume : MonoBehaviour
{
    public void SetVolume(float value)
    {
        SoundManager.BackgroundVolume = value;
    }
}
