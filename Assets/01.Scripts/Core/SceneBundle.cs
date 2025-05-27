using System.Threading.Tasks;
using UnityEngine;

public abstract class SceneBundle : MonoBehaviour
{
  public static SceneBundle CurrentBundle { get; protected set; } = null;

  public SerializableDictionary<string, DataSet> bundles = new();

  public static async Task<T> Load<T>(params DataBundle[] bundles) where T : SceneBundle
  {
    if(CurrentBundle)
    {
      await CurrentBundle.UnLoad();
      if(CurrentBundle) Destroy(CurrentBundle.gameObject);
    }
    
    var dataBundles = new SerializableDictionary<string, DataSet>();

    foreach (var dataBundle in bundles)
    {
      dataBundles[dataBundle.name] = await dataBundle.Load();
    }

    var bundleObject = new GameObject($"[{typeof(T).Name}_Bundle]");
    var bundle = bundleObject.AddComponent<T>();
    bundle.bundles = dataBundles;

    await bundle.PreLoad(dataBundles);

    CurrentBundle = bundle;
    
    bundle.Ready();

    return bundle;
  }

  public static T LoadSync<T>(params DataBundle[] bundles) where T : SceneBundle
  {
    var task = Load<T>(bundles);
    task.Wait();
    return task.Result;
  }

  /// <summary>
  /// 씬 번들을 불러오기 전 DataBundle 등 에셋들을 비동기로 불러오는 용도
  /// </summary>
  protected abstract Task PreLoad(SerializableDictionary<string, DataSet> bundles);
  
  /// <summary>
  /// PreLoad가 끝났을 시 호출되는 메소드
  /// </summary>
  protected abstract void Ready();

  /// <summary>
  /// SceneBundle이 언로드될 떄 호출되는 메소드
  /// 다른 SceneBundle를 호출하기 전 활성화되어있는 SceneBundle이 있을 시 호출함
  /// </summary>
  protected virtual Task UnLoad()
  {
    if(CurrentBundle == this) CurrentBundle = null;

    foreach (var bundle in bundles.Values)
    {
      bundle.Release();
    }
    
    return Task.CompletedTask;
  }
}