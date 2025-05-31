using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerSound : MonoBehaviour
{
    [Header("Sounds")]
    [SerializeField] private AudioClip[] walkSounds;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip[] hitSounds;
    [SerializeField] private AudioClip deadSound;
    [SerializeField] private AudioClip[] meleeSounds;
    [SerializeField] private AudioClip[] healSounds;
    [SerializeField] private AudioClip[] equipSounds;
    [SerializeField] private AudioClip[] lootSounds;
    
    [SerializeField] private ZombieHearingComponent _zombieHearingComponent;
    private Coroutine zombieHearingCoroutine;
    private float curSoundRadius = 0;
    
    private void Start()
    {
        InvokeRepeating("ZombieHearing", 0, 1f);
    }
    
    public void WalkSound()
    {
        int index = Random.Range(0, walkSounds.Length);
        string soundName = walkSounds[index].name;
        SoundManager.Play(soundName);
        ZombieHearingHandler(2.5f);
    }

    public void RunSound()
    {
        int index = Random.Range(0, walkSounds.Length);
        string soundName = walkSounds[index].name;
        SoundManager.Play(soundName);
        ZombieHearingHandler(5);
    }

    public void LandSound()
    {
        string soundName = jumpSound.name;
        SoundManager.Play(soundName);
        ZombieHearingHandler(5,2f);
    }

    public void HitSound()
    {
        int index = Random.Range(0, hitSounds.Length);
        string soundName = hitSounds[index].name;
        SoundManager.Play(soundName);
        ZombieHearingHandler(2);
    }

    public void DeadSound()
    {
        string soundName = deadSound.name;
        SoundManager.Play(soundName);
    }
    
    public void MeleeSound()
    {
        int index = Random.Range(0, meleeSounds.Length);
        string soundName = meleeSounds[index].name;
        SoundManager.Play(soundName);
        ZombieHearingHandler(5);
    }

    // public void RangedSound()
    // {
    //     string soundName = rangedSound.name;
    //     SoundManager.Play(soundName);
    //     ZombieHearingHandler(10);
    // }

    public void HealSound()
    {
        int index = Random.Range(0, healSounds.Length);
        string soundName = healSounds[index].name;
        SoundManager.Play(soundName);
    }

    public void EquipSound()
    {
        int index = Random.Range(0, equipSounds.Length);
        string soundName = equipSounds[index].name;
        SoundManager.Play(soundName);
    }

    public void LootSound()
    {
        int index = Random.Range(0, lootSounds.Length);
        string soundName = lootSounds[index].name;
        SoundManager.Play(soundName);
    }
    
    private void ZombieHearing()
    {
        _zombieHearingComponent.OnSound(curSoundRadius);
    }

    private void ZombieHearingHandler(float radius, float duration = 1f)
    {
        if (zombieHearingCoroutine != null)
        {
            StopCoroutine(zombieHearingCoroutine);
        }

        StartCoroutine(MakeSound(radius, duration));
    }

    private IEnumerator MakeSound(float radius, float duration)
    {
        curSoundRadius = radius;
        yield return new WaitForSeconds(duration);
    }
}
