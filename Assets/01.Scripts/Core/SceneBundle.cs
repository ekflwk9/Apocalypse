using System;
using System.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

public class SceneBundle : MonoBehaviour
{
  public static SceneBundle CurrentBundle { get; protected set; } = null;

  #region Properties
  [SerializeField] protected bool isUnloaded = false;
  private bool loadByLoader = false;

  /// <summary>
  /// SceneBundle 로딩전 데이터 번들을 로딩하고 Preload가 호출되기 전 데이터를 할당합니다.
  /// </summary>
  public SerializableDictionary<string, AssetData> bundles = new();
  
  /// <summary>
  /// 인스펙터에 설정해놨을 시 미리 로딩하는 에셋 번들입니다.
  /// </summary>
  [SerializeField] private AssetBundle[] preloadBundles;
  
  #endregion Properties

  #region Static Feature
  /// <summary>
  /// 비동기로 씬 데이터를 로딩합니다.
  /// </summary>
  /// <param name="bundles">SceneBundle를 불러오기 전 불러올 DataBundle 목록입니다.</param>
  /// <typeparam name="T">불러올 SceneBundle입니다.</typeparam>
  private static async Task LoadScene<T>(params AssetBundle[] bundles) where T : SceneBundle
  {
    if(CurrentBundle)
    {
      CurrentBundle.OnSceneEnd();
      CurrentBundle.isUnloaded = true;
      if(CurrentBundle) Destroy(CurrentBundle.gameObject);
    }
    
    var dataBundles = new SerializableDictionary<string, AssetData>();

    foreach (var dataBundle in bundles)
    {
      dataBundles[dataBundle.name] = await dataBundle.Load();
    }

    var bundleObject = new GameObject($"[{typeof(T).Name}_Bundle]");
    var bundle = bundleObject.AddComponent<T>();
    bundle.loadByLoader = true;
    bundle.bundles = dataBundles;
    bundle.OnScenePreLoad(dataBundles);

    CurrentBundle = bundle;
    
    bundle.OnSceneStart();
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
  /// 현재 활성화된 씬을 가져옵니다.
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
  
  #endregion Static Feature

  #region Interface

  /// <summary>
  /// 씬 번들을 불러오기 전 DataBundle 등 에셋들을 비동기로 불러오는 용도
  /// </summary>
  protected virtual void OnScenePreLoad(SerializableDictionary<string, AssetData> loadedAssets)
  {
  }

  /// <summary>
  /// PreLoad가 끝났을 시 호출되는 메소드
  /// </summary>
  protected virtual void OnSceneStart()
  {
  }

  /// <summary>
  /// SceneBundle이 언로드될 떄 호출되는 메소드
  /// 다른 SceneBundle를 호출하기 전 활성화되어있는 SceneBundle이 있을 시 호출함
  /// </summary>
  protected virtual void OnSceneEnd()
  {
    if(CurrentBundle == this) CurrentBundle = null;

    foreach (var bundle in bundles.Values)
    {
      bundle.Release();
    }
  }
  
  public T GetAsset<T>(string bundleName ,string key) where T : Object
  {
      if (bundles.TryGetValue(bundleName, out var bundle))
      {
          T asset = bundle.GetAsset<T>(key);
          return asset;
      } 
      else
      {
#if UNITY_EDITOR
            Debug.LogError($"Bundle {bundleName} does not exist");
#endif
            return null;
      }
  }

  public GameObject Instantiate(string bundleName, string key)
  {
      var asset = GetAsset<GameObject>(bundleName, key);
      return Instantiate(asset);
  }

  public T GetAsset<T>(string key) where T : Object
  {
      T asset = ContentManager.GetAsset<T>(key);
      return asset;
  }

  public GameObject Instantiate(string key)
  {
      var asset = GetAsset<GameObject>(key);
      return Instantiate(asset);
  }
  
  #endregion Interface
  
  #region Unity Event
  protected virtual void Awake()
  {
    foreach (var bundle in preloadBundles)
    {
      bundles[bundle.name] = bundle.LoadSync();
    }

    if (!loadByLoader)
    {
      OnScenePreLoad(bundles);
    
      OnSceneStart();
    }
  }

  protected virtual void OnDestroy()
  {
    if (!isUnloaded) OnSceneEnd();
  }
  
  #endregion Unity Event
}