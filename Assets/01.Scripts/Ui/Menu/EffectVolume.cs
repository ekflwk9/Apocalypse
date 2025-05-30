using UnityEngine;

public class EffectVolume : MonoBehaviour
{
    public void SetVolume(float value)
    {
        SoundManager.EffectVolume = value;
    }
}
