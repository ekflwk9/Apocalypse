using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

/// <summary>
/// AssetBundle의 에셋 참조를 읽어서 키(에셋 이름)-값(오브젝트) 형태로 메모리에 올립니다. 
/// </summary>
[Serializable]
public class AssetData
{
  /// <summary>
  /// 현재 활성화된 씬 데이터의 목록입니다.
  /// 내용은 ContentManager.Instance.activeData의 내용과 같습니다.
  /// </summary>
  public static Dictionary<string, AssetData> ActiveData { get; private set; } = new();
  
  #region Memory
  // 데이터 목록
  [SerializeField] private SerializableDictionary<string, Sprite> sprites = new();
  [SerializeField] private SerializableDictionary<string, AudioClip> audioClips = new();
  [SerializeField] private SerializableDictionary<string, GameObject> prefabs = new();
  [SerializeField] private SerializableDictionary<string, ScriptableObject> others = new();
  [SerializeField] private SerializableDictionary<string, Object> allAssets = new();
  
  public SerializableDictionary<string, Sprite> Sprites => sprites;
  public SerializableDictionary<string, AudioClip> AudioClips => audioClips;
  public SerializableDictionary<string, GameObject> Prefabs => prefabs;
  public SerializableDictionary<string, ScriptableObject> Others => others;
  public SerializableDictionary<string, Object> AllAssets => allAssets;
  #endregion

  /// <summary>
  /// 해당 인스턴스가 생성된 패키지의 이름입니다.
  /// </summary>
  [SerializeField] private string name;
  public string Name => name;
  /// <summary>
  /// 해당 명칭의 씬이 언로드됬을 때 해당 인스턴스가 Release됩니다.
  /// </summary>
  public string releaseScene;

  [SerializeField] private bool released = false;
  // 데이터가 해제되었을 시 true를 반환합니다.
  public bool Released => released;

  /// <summary>
  /// AssetData 인스턴스 전용으로 구현하기 위해 private로 구현했습니다.
  /// </summary>
  /// <param name="assets"></param>
  /// <param name="name"></param>
  /// <param name="releaseScene"></param>
  private AssetData(List<Object> assets, string name, string releaseScene ="")
  {
    this.releaseScene = releaseScene;
    this.name = name;

    foreach (var asset in assets)
    {
      allAssets[asset.name] = asset;

      switch (asset)
      {
        case Sprite sprite:
          sprites[sprite.name] = sprite;
          break;
        
        case AudioClip clip:
          audioClips[clip.name] = clip;
          break;
        
        case GameObject prefab:
          prefabs[prefab.name] = prefab;
          break;
        
        case ScriptableObject other:
          others[other.name] = other;
          break;
      }
    }

    SceneManager.sceneUnloaded += Unloader;
    return;

    void Unloader(Scene scene)
    {
      if (scene.name == this.releaseScene) Release();
      SceneManager.sceneUnloaded -= Unloader;
    }
  }

  /// <summary>
  ///  해제되지 않았을 시 해당 인스턴스를 메모리에서 해제시킵니다.
  ///  메모리 해제 이후 내부 데이터 접근 불가합니다.
  /// </summary>
  public void Release()
  {
    if (!released)
    {
      foreach (var pair in allAssets.ToArray())
      {
        Addressables.Release(pair.Value);
      }
      
      allAssets.Clear();
      prefabs.Clear();
      sprites.Clear();
      audioClips.Clear();
      others.Clear();
      
      allAssets = null;
      prefabs = null;
      sprites = null;
      audioClips = null;
      others = null;
      
      ActiveData.Remove(name);
      #if UNITY_EDITOR
      if(ContentManager.Instance) ContentManager.Instance.activeData.Remove(releaseScene);
      #endif
    }
    
    released = true;
  }

  /// <summary>
  /// 이 메소드로 에셋을 바로 가져올 수 있습니다.
  /// </summary>
  /// <param name="key">가져오려는 에셋의 이름입니다.</param>
  /// <param name="value"></param>
  /// <typeparam name="T"></typeparam>
  /// <returns></returns>
  public bool TryGet<T>(string key, out T value) where T : Object
  {
    if (allAssets.TryGetValue(key, out var asset))
    {
      value = asset as T;
      return true;
    }
    
    value = null;
    return false;
  }

  /// <summary>
  /// 
  /// </summary>
  /// <param name="key"></param>
  /// <typeparam name="T"></typeparam>
  /// <returns></returns>
  public T GetAsset<T>(string key) where T : Object
  {
    if(TryGet(key, out T value)) return value;
    throw new Exception($"AssetData does not contain {key}");
  }
  
  public GameObject Instantiate(string key)
  {
    if(Prefabs.TryGetValue(key, out var prefab)) return Object.Instantiate(prefab);
    
    throw new Exception($"AssetData does not contain {key}");
  }

  /// <summary>
  /// 생성자를 감추기 위해 어쩔 수 없이 AssetData에 구현했습니다.
  /// </summary>
  /// <param name="package">불러올 데이터의 패키지입니다.</param>
  /// <param name="releaseScene">해당 명칭의 씬이 언로드됬을 때 해당 데이터가 Release됩니다.</param>
  /// <param name="force">참일시 이름이 중복되는 기존 데이터가 있으면 삭제하고 생성합니다.</param>
  /// <returns></returns>
  public static async Task<AssetData> Create(AssetBundle package, string releaseScene = "", bool force = false)
  {
    if (force && ActiveData.TryGetValue(package.name, out var data)) data.Release();
    
    var loadedAssets = new List<Object>();
    
    // 현재 임시로 동기적으로 불러오게끔 구현해놨고, 이후 Task만 따로 분리하여 멀티스레드 로딩 구현이 필요합니다.
    // 에셋을 동기로 불러오고 null 아닐시 메모리에 올림
    foreach (var reference in package.assetReferences)
    {
      var asset = reference.LoadAssetAsync<Object>().WaitForCompletion();
      
      if (asset != null)
        loadedAssets.Add(asset);
    }

    // 라벨 붙은 에셋들을 동기로 불러오고 null이 아닐시 메모리에 올림
    foreach (var label in package.assetLabelReferences)
    {
      Addressables.LoadAssetsAsync<Object>(label, obj =>
      {
        if (obj != null) loadedAssets.Add(obj);
      }).WaitForCompletion();
    }

    var result = new AssetData(loadedAssets, package.name, releaseScene);
    ActiveData[package.name] = result;
    
    #if UNITY_EDITOR
    if(ContentManager.Instance) ContentManager.Instance.activeData[package.name] = result;
    #endif

    return result;
  }
  
  /// <summary>
  /// Create의 동기 버전입니다.
  /// </summary>
  /// <param name="package">불러올 데이터의 패키지입니다.</param>
  /// <param name="releaseScene">해당 명칭의 씬이 언로드됬을 때 해당 데이터가 해제됩니다.</param>
  /// <param name="force">참일시 이름이 중복되는 기존 데이터가 있으면 삭제하고 생성합니다.</param>
  /// <returns></returns>
  public static AssetData CreateSync(AssetBundle package, string releaseScene = "", bool force = false)
  {
    var data = Create(package, releaseScene, force);
    data.Wait();
    return data.Result;
  }

  /// <summary>
  /// 해당 인스턴스가 해제되었는지 확인하는 코드입니다.
  /// </summary>
  public static explicit operator bool(AssetData assetData) => !assetData.released;
  
  public static implicit operator SerializableDictionary<string, Object>(AssetData data) => data.AllAssets;
}