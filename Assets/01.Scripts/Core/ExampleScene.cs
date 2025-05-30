using System;
using System.Collections;
using UnityEngine;

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
        var item = ContentManager.Instantiate("Consumable_MedicPack");
        Debug.Log(SoundManager.Mixer);
        break;
      }
      yield return null;
    }
  }

  #region Unity Events

  protected override void Awake()
  {
      base.Awake();
  }

  protected override void OnDestroy()
  {
      base.OnDestroy();
  }

  #endregion
}