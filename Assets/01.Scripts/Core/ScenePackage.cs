using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "new ScenePackage", menuName = "Apocalypse/ScenePackage", order = 0)]
public class ScenePackage : ScriptableObject
{
  public AssetReference[] assetReferences;
  public AssetLabelReference[] assetLabelReferences;
  public GameObject[] prefab;

  /// <summary>
  /// SceneData의 Create를 단순히 사용하기 쉽게 가져왔습니다.
  /// 동기로 사용하려면 CreateSync를 사용하세요.
  /// </summary>
  /// <param name="releaseScene">해당 명칭의 씬이 언로드됬을 때 해당 데이터가 Release됩니다.</param>
  /// <param name="force">참일시 이름이 중복되는 기존 데이터가 있으면 삭제하고 생성합니다.</param>
  /// <returns></returns>
  public async Task<SceneData> Load(string releaseScene = "", bool force = false)
  {
    return await SceneData.Create(this, releaseScene, force);
  }

  /// <summary>
  /// Load의 동기 버전입니다.
  /// </summary>
  /// <param name="releaseScene">해당 명칭의 씬이 언로드됬을 때 해당 데이터가 Release됩니다.</param>
  /// <param name="force">참일시 이름이 중복되는 기존 데이터가 있으면 삭제하고 생성합니다.</param>
  /// <returns></returns>
  public SceneData LoadSync(string releaseScene = "", bool force = false)
  {
    var task = Load(releaseScene, force);
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
}