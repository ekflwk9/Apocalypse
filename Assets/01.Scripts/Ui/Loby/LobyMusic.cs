using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobyMusic : MonoBehaviour
{
    private void Awake()
    {
        SoundManager.BackgroundPaused = true;
        SoundManager.Play("Loby_BackGround", SoundType.Background);
    }
}
