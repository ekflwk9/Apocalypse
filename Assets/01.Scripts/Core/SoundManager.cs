using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Audio;

/// <summary>
/// 싱글톤보다 정적 클래스가 접근하기 편할 것 같아 정적 클래스로 구현했습니다.
/// 각 값은 SoundManager 초기화시 자동으로 불러오며, 값을 설정시 해당 값을 자동으로 저장합니다.
/// </summary>
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

  /// <summary>
  /// 각 볼륨을 총괄하는 마스터 볼륨입니다.
  /// 해당 볼륨을 변경시 다른 모든 볼륨이 영향받습니다.
  /// 값은 0~100 사이로 설정할 수 있고, 미만 혹은 초과시 자동으로 포맷팅됩니다.
  /// </summary>
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

  /// <summary>
  /// 배경 볼륨입니다.
  /// 값은 0~100 사이로 설정할 수 있고, 미만 혹은 초과시 자동으로 포맷팅됩니다.
  /// </summary>
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

  /// <summary>
  /// 효과음 볼륨입니다.
  /// 값은 0~100 사이로 설정할 수 있고, 미만 혹은 초과시 자동으로 포맷팅됩니다.
  /// </summary>
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