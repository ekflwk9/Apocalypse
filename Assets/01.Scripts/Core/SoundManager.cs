using System;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Audio;

/// <summary>
/// 싱글톤보다 정적 클래스가 접근하기 편할 것 같아 정적 클래스로 구현했습니다.
/// 각 값은 SoundManager 초기화시 자동으로 불러오며, 값을 설정시 해당 값을 자동으로 저장합니다.
/// </summary>
public static class SoundManager
{
    private delegate float Oper(float a);
  public static AudioMixer mixer;
  private static bool loaded = false;
  public static bool Loaded => loaded;

  private static AudioSource effectSource, backgroundSource;
  private static AudioMixerGroup effectGroup, backgroundGroup, objectGroup;
  
  static SoundManager()
  {
    Load();
  }
  
  public static void Load()
  {
      if(loaded) return;
      loaded = true;
      
      Mixer = Addressables.LoadAssetAsync<AudioMixer>(new AssetLabelReference{labelString = "AudioMixer"}).WaitForCompletion();
  }

  public static AudioMixer Mixer
  {
      get => mixer;
      set
      {
          mixer = value;
          if (mixer)
          {
              MasterVolume = PlayerPrefs.GetFloat("MasterVolume", 80);
              BackgroundVolume = PlayerPrefs.GetFloat("BackgroundVolume", 52);
              EffectVolume = PlayerPrefs.GetFloat("EffectVolume", 60);
              
              effectGroup = Mixer.FindMatchingGroups("Effect")[0];
              backgroundGroup = Mixer.FindMatchingGroups("Background")[0];
              objectGroup = Mixer.FindMatchingGroups("Object")[0];
          }
      }
  }

  private static readonly Oper To = v => v - 80;
  private static readonly Oper From = v => v + 80;

  /// <summary>
  /// 각 볼륨을 총괄하는 마스터 볼륨입니다.
  /// 해당 볼륨을 변경시 다른 모든 볼륨이 영향받습니다.
  /// 값은 0~100 사이로 설정할 수 있고, 미만 혹은 초과시 자동으로 포맷팅됩니다.
  /// </summary>
  public static float MasterVolume
  {
    get => Mixer && Mixer.GetFloat("Master", out var value) ? From(value) : PlayerPrefs.GetFloat("MasterVolume", 80);
    set
    {
      var input = Math.Max(0, Math.Min(100, value));

      if(Mixer) Mixer.SetFloat("Master", To(value));
      PlayerPrefs.SetFloat("MasterVolume", input);
    }
  }
  
  /// <summary>
  /// 배경 볼륨입니다.
  /// 값은 0~100 사이로 설정할 수 있고, 미만 혹은 초과시 자동으로 포맷팅됩니다.
  /// </summary>
  public static float BackgroundVolume
  {
      get => PlayerPrefs.GetFloat("BackgroundVolume", 80);
      set
      {
          var input = Math.Max(0, Math.Min(100, value));

          if (BackgroundSource) BackgroundSource.volume = input / 100;
          PlayerPrefs.SetFloat("BackgroundVolume", input);
      }
  }

  /// <summary>
  /// 효과음 볼륨입니다.
  /// 값은 0~100 사이로 설정할 수 있고, 미만 혹은 초과시 자동으로 포맷팅됩니다.
  /// </summary>
  public static float EffectVolume
  {
    get => PlayerPrefs.GetFloat("EffectVolume", 80);
    set
    {
      var input = Math.Max(0, Math.Min(100, value));

      if(EffectSource) EffectSource.volume = input / 100;
      PlayerPrefs.SetFloat("EffectVolume", input);
    }
  }
  
  /// <summary>
  /// 오브젝트 볼륨입니다.
  /// 값은 0~100 사이로 설정할 수 있고, 미만 혹은 초과시 자동으로 포맷팅됩니다.
  /// </summary>
  public static float ObjectVolume
  {
    get => Mixer && Mixer.GetFloat("Object", out var value) ? From(value) : PlayerPrefs.GetFloat("ObjectVolume", 80);
    set
    {
      var input = Math.Max(0, Math.Min(100, value));

      if(Mixer) Mixer.SetFloat("Object", To(value));
      PlayerPrefs.SetFloat("ObjectVolume", input);
    }
  }

  public static AudioSource EffectSource
  {
    get
    {
      var cam = Camera.main;
      if (cam == null) return null;

      if (effectSource && effectSource.gameObject == cam.gameObject)
      {
          effectSource.volume = EffectVolume / 100;
        return effectSource;
      }
      
      AudioSource source = cam.GetComponents<AudioSource>().FirstOrDefault(s => s.outputAudioMixerGroup == effectGroup);

      if(!source || source != effectSource)
      {
        source = effectSource = cam.gameObject.AddComponent<AudioSource>();
        effectSource.outputAudioMixerGroup = effectGroup;
      }
      
      source.volume = EffectVolume / 100;

      return source;
    }
  }
  
  public static AudioSource BackgroundSource
  {
    get
    {
      var cam = Camera.main;
      if (!cam) return null;
      
      if (backgroundSource && backgroundSource.gameObject == cam.gameObject)
      {
          backgroundSource.volume = BackgroundVolume / 100;
        return backgroundSource;
      }
      
      AudioSource source = cam.GetComponents<AudioSource>().FirstOrDefault(s => s.outputAudioMixerGroup == backgroundGroup);

      if(!source || source != backgroundSource)
      {
        source = backgroundSource = cam.gameObject.AddComponent<AudioSource>();
        source.loop = true;
        backgroundSource.outputAudioMixerGroup = backgroundGroup;
      }
      
      source.volume = BackgroundVolume / 100;

      return source;
    }
  }

  public static AudioSource Play(string clipName, GameObject obj)
  {
    if(!Mixer) return null;
    
    AudioSource source = null;

    foreach (var audioSource in obj.GetComponents<AudioSource>())
    {
        if (audioSource.outputAudioMixerGroup == objectGroup && audioSource.isPlaying == false)
        {
            source = audioSource;
            break;
        }
    }
    
    if (!source)
    {
        source = obj.AddComponent<AudioSource>();
        source.outputAudioMixerGroup = objectGroup;
        source.spatialBlend = 1;
    }
    
    source.clip = ContentManager.GetAsset<AudioClip>(clipName);
    source.Play();

    return source;
  }

  public static AudioSource Play(string clipName, SoundType type = SoundType.Effect)
  {
    if(!Mixer) return null;
    
    AudioSource source = type switch
    {
      SoundType.Background => BackgroundSource,
      SoundType.Effect => EffectSource,
      _ => EffectSource
    };
    
    source.clip = ContentManager.GetAsset<AudioClip>(clipName);
    source.outputAudioMixerGroup = type == SoundType.Background ? backgroundGroup : effectGroup;
    source.Play();
    
    return source;
  }

  public static bool EffectPaused
  {
      get => EffectSource.isPlaying;
      set
      {
          if(value) EffectSource.Pause();
          else EffectSource.UnPause();
      }
  }
  
  public static bool BackgroundPaused
  {
      get => backgroundSource.isPlaying;
      set
      {
          if(value) BackgroundSource.Pause();
          else BackgroundSource.UnPause();
      }
  }

  public static GameObject PauseAudio(this GameObject obj)
  {
      if (obj && obj.TryGetComponent(out AudioSource source))
      {
          source.Pause();
      }
      return obj;
  }
  
  public static GameObject ResumeAudio(this GameObject obj)
  {
      if (obj && obj.TryGetComponent(out AudioSource source))
      {
          source.UnPause();
      }
      return obj;
  }
  
  public static GameObject StopAudio(this GameObject obj)
  {
      if (obj && obj.TryGetComponent(out AudioSource source))
      {
          source.Stop();
      }

      return obj;
  }

  public static GameObject PlayAudio(this GameObject obj, string clipName)
  {
      if (obj) Play(clipName, obj);

      return obj;
  }
}

public enum SoundType
{
  Background,
  Effect
}