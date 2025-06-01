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

    [SerializeField] AudioClip HitClip_1;
    void Hit()
    {
        audioSource.clip = HitClip_1;
        audioSource.Play();
    }

    [SerializeField] AudioClip AttackClip;

    void OnAttackSound()
    {
        audioSource.clip = AttackClip;
        audioSource.Play();
    }

    [SerializeField] AudioClip HurtClip_1;
    [SerializeField] AudioClip HurtClip_2;

    void Hurt()
    {
        int value = Random.Range(1, 3);
        if (value == 1)
        {
            audioSource.clip = HurtClip_1;
        }
        else
        {
            audioSource.clip = HurtClip_2;
        }
        audioSource.Play();
    }

    [SerializeField] AudioClip DieClip;

    void Die()
    {
        audioSource.clip = DieClip;
        audioSource.Play();
    }

}
