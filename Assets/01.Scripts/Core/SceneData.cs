using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

[Serializable]
public class SceneData
{
  public bool releaseOnSceneUnload;
  // 데이터 목록
  private List<object> assets;
  [SerializeField] private bool released = false;
  public bool Released => released;
  
  public SceneData(List<object> assets, bool releaseOnSceneUnload = true)
  {
    this.assets = assets;
    this.releaseOnSceneUnload = releaseOnSceneUnload;
    
    


    SceneManager.sceneUnloaded += Unloader;
    return;

    void Unloader(Scene _)
    {
      if (this.releaseOnSceneUnload) Release();
      SceneManager.sceneUnloaded -= Unloader;
    }
  }

  /// <summary>
  ///  release 되지 않았을 시 release함.
  /// </summary>
  public void Release()
  {
    if (!released)
    {
      foreach (var asset in assets.ToArray())
      {
        Addressables.Release(asset);
      }
    }
    
    released = true;
  }
}