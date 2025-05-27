using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public abstract class SceneBundle : MonoBehaviour
{
  public static SceneBundle CurrentBundle { get; protected set; } = null;

  /// <summary>
  /// SceneBundle 로딩전 데이터 번들을 로딩하고 Preload가 호출되기 전 데이터를 할당합니다.
  /// </summary>
  public SerializableDictionary<string, AssetData> bundles = null;

  /// <summary>
  /// 비동기로 씬 데이터를 로딩합니다.
  /// </summary>
  /// <param name="bundles">SceneBundle를 불러오기 전 불러올 DataBundle 목록입니다.</param>
  /// <typeparam name="T">불러올 SceneBundle입니다.</typeparam>
  private static async Task LoadScene<T>(params AssetBundle[] bundles) where T : SceneBundle
  {
    if(CurrentBundle)
    {
      CurrentBundle.UnLoad();
      if(CurrentBundle) Destroy(CurrentBundle.gameObject);
    }
    
    var dataBundles = new SerializableDictionary<string, AssetData>();

    foreach (var dataBundle in bundles)
    {
      dataBundles[dataBundle.name] = await dataBundle.Load();
    }

    var bundleObject = new GameObject($"[{typeof(T).Name}_Bundle]");
    var bundle = bundleObject.AddComponent<T>();
    bundle.bundles = dataBundles;

    var loader = bundle.PreLoad(dataBundles);

    for (var i = 0; i < 100; i++)
      if (!loader.MoveNext()) break;

    CurrentBundle = bundle;
    
    bundle.Ready();
  }

  /// <summary>
  /// 동기로 씬 데이터를 로딩합니다.
  /// </summary>
  /// <param name="bundles">SceneBundle를 불러오기 전 불러올 DataBundle 목록입니다.</param>
  /// <typeparam name="T">불러올 SceneBundle입니다.</typeparam>
  public static void LoadSceneSync<T>(params AssetBundle[] bundles) where T : SceneBundle
  {
    LoadScene<T>(bundles).Wait();
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="sceneBundle"></param>
  /// <typeparam name="T"></typeparam>
  /// <returns></returns>
  public static bool TryGetCurrentScene<T>(out T sceneBundle) where T : SceneBundle
  {
    sceneBundle = CurrentBundle as T;
    return sceneBundle;
  }

  /// <summary>
  /// 현재 활성화된 씬 데이터를 가져옵니다.
  /// </summary>
  /// <typeparam name="T">활성화된 씬의 타입입니다.</typeparam>
  /// <returns>활성화된 씬의 인스턴스입니다.</returns>
  public static T GetCurrentScene<T>() where T : SceneBundle
  {
    if (TryGetCurrentScene<T>(out var scene)) return scene;
    else throw new Exception($"{typeof(T).Name}는 현재 활성화된 씬이 아닙니다.");
  }

  /// <summary>
  /// 씬 번들을 불러오기 전 DataBundle 등 에셋들을 비동기로 불러오는 용도
  /// </summary>
  protected abstract IEnumerator PreLoad(SerializableDictionary<string, AssetData> loadedAssets);
  
  /// <summary>
  /// PreLoad가 끝났을 시 호출되는 메소드
  /// </summary>
  protected abstract void Ready();

  /// <summary>
  /// SceneBundle이 언로드될 떄 호출되는 메소드
  /// 다른 SceneBundle를 호출하기 전 활성화되어있는 SceneBundle이 있을 시 호출함
  /// </summary>
  protected virtual void UnLoad()
  {
    if(CurrentBundle == this) CurrentBundle = null;

    foreach (var bundle in bundles.Values)
    {
      bundle.Release();
    }
  }
}