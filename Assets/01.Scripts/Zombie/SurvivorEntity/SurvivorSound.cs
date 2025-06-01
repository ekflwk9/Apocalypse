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
    }

    const string HitSound = "Survivor_Hit_";
    void Hit()
    {
        int value = Random.Range(1, 3);
        gameObject.PlayAudio(HitSound + value.ToString());
    }

    const string AttackSound = "SurvivorAttack";

    void OnAttackSound()
    {
        gameObject.PlayAudio(AttackSound);
    }


    void Hurt()
    {
        int value = Random.Range(1, 3);
        gameObject.PlayAudio(HitSound + value.ToString());
    }

    void Die()
    {
        gameObject.PlayAudio("SurvivorDead");
    }

}
