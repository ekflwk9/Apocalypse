using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivorSound : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;
    private void Reset()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.loop = false;
            audioSource.playOnAwake = false;

            audioSource.rolloffMode = AudioRolloffMode.Custom;
            audioSource.spatialBlend = 1;
            audioSource.maxDistance = 8f;
            audioSource.minDistance = 2f;
        }
        else
        {
            audioSource.loop = false;
            audioSource.playOnAwake = false;

            audioSource.rolloffMode = AudioRolloffMode.Custom;
            audioSource.spatialBlend = 1;
            audioSource.maxDistance = 8f;
            audioSource.minDistance = 2f;
        }
    }

    const string HitSound = "Survivor_Hit_";

    void Hit()
    {
        int value = Random.Range(1, 3);
        SoundManager.Play(HitSound + value.ToString());
    }

    const string AttackSound = "survivor_Attack";

    void OnAttackSound()
    {
        SoundManager.Play(AttackSound);
    }


    void Hurt()
    {
        int value = Random.Range(1, 3);
        SoundManager.Play(HitSound + value.ToString());
    }

    void Die()
    {
        SoundManager.Play("SurvivorDead");
    }

}
