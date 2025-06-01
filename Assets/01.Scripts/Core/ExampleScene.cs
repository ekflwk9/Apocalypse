using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExampleScene : SceneBundle
{
  #region SceneBundle Interface

  protected override void OnScenePreLoad(SerializableDictionary<string, AssetData> loadedAssets)
  {
  }

  protected override void OnSceneStart()
  {
    StartCoroutine(Load(() => ContentManager.Loaded));
  }

  protected override void OnSceneEnd()
  {
    base.OnSceneEnd();
  }

  #endregion

  private IEnumerator Load(Func<bool> trigger)
  {
    for (;;)
    {
      if (trigger())
      {
          gameObject.SendMessage("Ready", SendMessageOptions.DontRequireReceiver);
        // SoundManager.Play("BasicBackGround", SoundType.Background);
        // SoundManager.Play("Walk_3", SoundType.Effect);
        break;
      }
      yield return null;
    }
  }

  #region Unity Events

  protected override void Awake()
  {
      base.Awake();
      SoundManager.Load();
  }

  protected override void OnDestroy()
  {
      base.OnDestroy();
  }

  #endregion
}