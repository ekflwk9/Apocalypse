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
            audioSource.maxDistance = 8f;
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

    const string IdleSound = "Zombie_Idle_";

    void UsuallySound()
    {
        int RandomValue = Random.Range(1, 3);
        string soundName = IdleSound + RandomValue.ToString();
        SoundManager.Play(soundName);
        SoundReset();
    }

    const string HitSound = "Zombie_Hit";

    void Hit()
    {
        SoundManager.Play(HitSound);
        SoundReset();
    }

    const string AttackSound1 = "Zombie_Attack_1";
    const string AttackSound2 = "Zombie_Attack_2";

    void AttackSound_1()
    {
        SoundManager.Play(AttackSound1);
        SoundReset();
    }

    void AttackSound_2()
    {
        SoundManager.Play(AttackSound2);
        SoundReset();
    }

    void Ranged()
    {
        SoundManager.Play(HitSound);
        SoundReset();
    }

    const string HurtSound = "Zombie_Hurt";

    const string DieSound = "Zombie_Die";

    void Hurt()
    {
        SoundManager.Play(HurtSound);
        SoundReset();
    }

    void Die()
    {
        SoundManager.Play(DieSound);
        SoundReset();
    }

    const string YellingSound = "Zombie_Yelling_";

    void Yelling()
    {
        int RandomValue = Random.Range(1, 3);
        string soundName = YellingSound + RandomValue.ToString();
        SoundManager.Play(soundName);
        SoundReset();
    }

    void SoundReset()
    {
        CurrentSoundTime = SoundTime;
    }

}
