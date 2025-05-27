using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "new Asset Bundle", menuName = "Apocalypse/Asset Bundle")]
public class AssetBundle : ScriptableObject
{
  public const string DefaultLabel = "Bundle";
  public AssetReference[] assetReferences;
  public AssetLabelReference[] assetLabelReferences;

  /// <summary>
  /// DataSet의 Create를 단순히 사용하기 쉽게 가져왔습니다.
  /// 동기로 사용하려면 CreateSync를 사용하세요.
  /// </summary>
  /// <param name="releaseScene">해당 명칭의 씬이 언로드됬을 때 해당 데이터가 해제됩니다.</param>
  /// <param name="force">참일시 이름이 중복되는 기존 데이터가 있으면 삭제하고 생성합니다.</param>
  /// <returns></returns>
  public async Task<AssetData> Load(string releaseScene = "", bool force = false)
  {
    return await AssetData.Create(this, releaseScene, force);
  }

  /// <summary>
  /// Load의 동기 버전입니다.
  /// </summary>
  /// <param name="releaseScene">해당 명칭의 씬이 언로드됬을 때 해당 데이터가 Release됩니다.</param>
  /// <param name="force">참일시 이름이 중복되는 기존 데이터가 있으면 삭제하고 생성합니다.</param>
  /// <returns></returns>
  public AssetData LoadSync(string releaseScene = "", bool force = false)
  {
    var task = Load(releaseScene, force);
    task.Wait();
    return task.Result;
  }

  /// <summary>
  /// ContentManager.LoadBundle과 동일하게 작동하는 메소드입니다.
  /// 전체 에셋을 불러오고 다시 비활성화하기 때문에 사용을 추천하지 않습니다.
  /// </summary>
  /// <param name="name">불러올 DataBundle의 파일명입니다.</param>
  /// <param name="label">불러올 DataBundle의 어드레서블 라벨입니다.</param>
  public static Task<AssetBundle> Find(string name, AssetLabelReference label)
    => ContentManager.LoadBundle(name, label);
  
  /// <summary>
  /// ContentManager.LoadBundle과 동일하게 작동하는 메소드입니다.
  /// 전체 에셋을 불러오고 다시 비활성화하기 때문에 사용을 추천하지 않습니다.
  /// </summary>
  /// <param name="name">불러올 DataBundle의 파일명입니다.</param>
  /// <param name="label">불러올 DataBundle의 문자열 라벨입니다.</param>
  public static Task<AssetBundle> Find(string name, string label) => Find(name, new AssetLabelReference{labelString = label});

  /// <summary>
  /// ContentManager.LoadBundle과 동일하게 작동하는 메소드입니다.
  /// 전체 에셋을 불러오고 다시 비활성화하기 때문에 사용을 추천하지 않습니다.
  /// </summary>
  /// <param name="name">불러올 DataBundle의 파일명입니다.</param>
  public static Task<AssetBundle> Find(string name)
  {
    AssetLabelReference label;

    if (ContentManager.Instance) label = ContentManager.Instance.bundleLabel;
    else label = new AssetLabelReference{labelString = AssetBundle.DefaultLabel};
    return ContentManager.LoadBundle(name, label);
  }
  
  /// <summary>
  /// ContentManager.LoadBundleSync와 동일하게 작동하는 메소드입니다.
  /// 전체 에셋을 불러오고 다시 비활성화하기 때문에 사용을 추천하지 않습니다.
  /// </summary>
  /// <param name="name">불러올 DataBundle의 파일명입니다.</param>
  /// <param name="label">불러올 DataBundle의 어드레서블 라벨입니다.</param>
  public static AssetBundle FindSync(string name, AssetLabelReference label)
  {
    var task = Find(name, label);
    task.Wait();
    return task.Result;
  }
  
  /// <summary>
  /// ContentManager.LoadBundleSync와 동일하게 작동하는 메소드입니다.
  /// 전체 에셋을 불러오고 다시 비활성화하기 때문에 사용을 추천하지 않습니다.
  /// </summary>
  /// <param name="name">불러올 DataBundle의 파일명입니다.</param>
  /// <param name="label">불러올 DataBundle의 문자열 라벨입니다.</param>
  public static AssetBundle FindSync(string name, string label) => FindSync(name, new AssetLabelReference{labelString = label});

  /// <summary>
  /// ContentManager.LoadBundleSync와 동일하게 작동하는 메소드입니다.
  /// 전체 에셋을 불러오고 다시 비활성화하기 때문에 사용을 추천하지 않습니다.
  /// </summary>
  /// <param name="name">불러올 DataBundle의 파일명입니다.</param>
  public static AssetBundle FindSync(string name)
  {
    var task = Find(name);
    task.Wait();
    return task.Result;
  }

  /// <summary>
  /// 메모리 최적화를 위해 데이터를 불러온 이후 호출하여 패키지데이터를 메모리에서 해제해주세요.
  /// </summary>
  public void Release()
  {
    Addressables.Release(this);
  }
  
  public static implicit operator AssetData(AssetBundle bundle) => bundle.LoadSync();
  public static implicit operator AssetBundle(string name) => FindSync(name);
}