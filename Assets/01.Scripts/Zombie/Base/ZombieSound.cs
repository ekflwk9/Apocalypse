using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSound : MonoBehaviour
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
            audioSource.maxDistance = 20f;
            audioSource.minDistance = 2f;
        }
    }

    const float SoundTime = 10f;
    float CurrentSoundTime = 10f;

    private void Update()
    {
        if (0 < CurrentSoundTime)
        {
            CurrentSoundTime -= Time.deltaTime;
        }
        else
        {
            UsuallySound();
            CurrentSoundTime = SoundTime;
        }
    }


    [SerializeField] AudioClip IdleClip_1;
    [SerializeField] AudioClip IdleClip_2;

    void UsuallySound()  // 2
    {
        int RandomValue = Random.Range(1, 3);
        if (RandomValue == 1)
        {
            audioSource.clip = IdleClip_1;
        }
        else
        {
            audioSource.clip = IdleClip_2;
        }
        audioSource.Play();
        SoundReset();
    }

    [SerializeField] AudioClip HitClip;

    void Hit()   // 1
    {
        audioSource.clip = HitClip;
        audioSource.Play();
        SoundReset();
    }

    [SerializeField] AudioClip AttackClip_1;
    [SerializeField] AudioClip AttackClip_2;

    void AttackSound_1()  // 2
    {
        audioSource.clip = AttackClip_1;
        audioSource.Play();
        SoundReset();
    }

    void AttackSound_2()
    {
        audioSource.clip = AttackClip_2;
        audioSource.Play();
        SoundReset();
    }

    [SerializeField] AudioClip RangeClip;

    void Ranged()  // 1
    {
        audioSource.clip = RangeClip;
        audioSource.Play();
        SoundReset();
    }

    [SerializeField] AudioClip HurtClip;
    [SerializeField] AudioClip DieClip;

    void Hurt()  // 1
    {
        audioSource.clip = HurtClip;
        audioSource.Play();
        SoundReset();
    }

    void Die()  // 1
    {
        audioSource.clip = DieClip;
        audioSource.Play();
        SoundReset();
    }

    [SerializeField] AudioClip YellingClip_1;
    [SerializeField] AudioClip YellingClip_2;

    void Yelling()  // 1
    {

        int RandomValue = Random.Range(1, 3);
        if(RandomValue == 1)
        {
            audioSource.clip = YellingClip_1;
        }
        else
        {
            audioSource.clip = YellingClip_2;
        }
        audioSource.Play();
        SoundReset();
    }

    void SoundReset()
    {
        CurrentSoundTime = SoundTime;
    }

}
