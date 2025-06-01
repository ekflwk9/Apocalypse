using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

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
            audioSource.minDistance = 4f;
        }
    }

    const float SoundTime = 10f;
    float CurrentSoundTime = 10f;

    float CurrentLocationTime = 0f;
    const float LocationTime = 1f;

    private void Update()
    {
        if (CurrentLocationTime < LocationTime)
        {
            CurrentLocationTime += Time.deltaTime;
        }
        else
        {
            CurrentLocationTime = 0f;
            Vector3 location = gameObject.transform.localPosition;
            location.y = 0;
            gameObject.transform.localPosition = location;
        }

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
        gameObject.PlayAudio(soundName);
        SoundReset();
    }

    const string HitSound = "Zombie_Hit";

    void Hit()
    {
        gameObject.PlayAudio(HitSound);
        SoundReset();
    }

    const string AttackSound1 = "Zombie_Attack_1";
    const string AttackSound2 = "Zombie_Attack_2";

    void AttackSound_1()
    {
        gameObject.PlayAudio(AttackSound1);
        SoundReset();
    }

    void AttackSound_2()
    {
        gameObject.PlayAudio(AttackSound2);
        SoundReset();
    }

    void Ranged()
    {
        gameObject.PlayAudio(HitSound);
        SoundReset();
    }

    const string HurtSound = "Zombie_Hurt";

    const string DieSound = "Zombie_Die";

    void Hurt()
    {
        gameObject.PlayAudio(HurtSound);
        SoundReset();
    }

    void Die()
    {
        gameObject.PlayAudio(DieSound);
        SoundReset();
    }

    const string YellingSound = "Zombie_Yelling_";

    void Yelling()
    {
        int RandomValue = Random.Range(1, 3);
        string soundName = YellingSound + RandomValue.ToString();
        gameObject.PlayAudio(soundName);
        SoundReset();
    }

    void SoundReset()
    {
        CurrentSoundTime = SoundTime;
    }

}
