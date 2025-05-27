using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Audio;

public static class SoundManager
{
  public static AudioMixer Mixer;

  static SoundManager()
  {
    Mixer = Addressables.LoadAssetAsync<AudioMixer>(new AssetReference("AudioMixer")).WaitForCompletion();

    if (Mixer)
    {
      MasterVolume = PlayerPrefs.GetFloat("MasterVolume", 80);
      BackgroundVolume = PlayerPrefs.GetFloat("BackgroundVolume", 80);
      EffectVolume = PlayerPrefs.GetFloat("EffectVolume", 80);
    }
  }

  public static float MasterVolume
  {
    get => Mixer && Mixer.GetFloat("Master", out var value) ? value + 80 : PlayerPrefs.GetFloat("MasterVolume", 80);
    set
    {
      var input = Math.Max(0, Math.Min(100, value));

      if(Mixer) Mixer.SetFloat("Master", input - 80);
      PlayerPrefs.SetFloat("MasterVolume", input);
    }
  }

  public static float BackgroundVolume
  {
    get => Mixer && Mixer.GetFloat("Background", out var value) ? value + 80 : PlayerPrefs.GetFloat("BackgroundVolume", 80);
    set
    {
      var input = Math.Max(0, Math.Min(100, value));

      if(Mixer) Mixer.SetFloat("Background", input - 80);
      PlayerPrefs.SetFloat("BackgroundVolume", input);
    }
  }

  public static float EffectVolume
  {
    get => Mixer && Mixer.GetFloat("Effect", out var value) ? value + 80 : PlayerPrefs.GetFloat("EffectVolume", 80);
    set
    {
      var input = Math.Max(0, Math.Min(100, value));

      if(Mixer) Mixer.SetFloat("Effect", input - 80);
      PlayerPrefs.SetFloat("EffectVolume", input);
    }
  }
}