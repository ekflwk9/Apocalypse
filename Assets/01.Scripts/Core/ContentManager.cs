using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Serialization;

public sealed class ContentManager : MonoBehaviour
{
  public static ContentManager Instance { get; private set; }
  
  /// <summary>
  ///   게임중 상시 메모리에 올라가있는 데이터
  /// </summary>
  [SerializeField] private DataSet sharedData;
  public DataSet SharedData => sharedData;
  
  // 프로젝트에서 영구적으로 메모리상에 올려둘 에셋 목록
  public DataBundle sharedPackage;
  
  // 패키지의 라벨 참조입니다. 패키지를 전역적으로 불러올 때 사용합니다.
  public AssetLabelReference bundleLabel = new() {labelString = DataBundle.DefaultLabel};
  
  #if UNITY_EDITOR
  // 패키지를 불러왔을 시 올려놓는 메모리입니다.
  // 인스펙터용 변수이므로 실제 구현은 static SceneData.ActiveData를 참조해주세요
  public SerializableDictionary<string, DataSet> activeData = new ();
  #endif

  /// <summary>
  ///   로딩하고 싶은 데이터가 있을 시 여기에 추가
  /// </summary>
  private void Load()
  {
    if (sharedPackage)
    {
      sharedData = sharedPackage.LoadSync();
    }
  }

  #region BundleLoader
  /// <summary>
  /// 전체 에셋을 불러오고 다시 비활성화하기 때문에 사용을 추천하지 않습니다.
  /// </summary>
  /// <param name="bundleName">불러올 DataBundle의 파일명입니다.</param>
  /// <param name="label">불러올 DataBundle의 어드레서블 라벨입니다.</param>
  public static async Task<DataBundle> LoadBundle(string bundleName, AssetLabelReference label)
  {
    var result = (await Addressables.LoadAssetsAsync<DataBundle>(label, null).Task).ToArray();
    
    if (result.Length == 0)
    {
      Debug.LogError("Package Not Found");
      return null;
    }

    return (from bundle in result where bundle.name == bundleName select bundle).First();
  }
  
  /// <summary>
  /// 전체 에셋을 불러오고 다시 비활성화하기 때문에 사용을 추천하지 않습니다.
  /// </summary>
  /// <param name="bundleName">불러올 DataBundle의 파일명입니다.</param>
  /// <param name="label">불러올 DataBundle의 어드레서블 문자열 라벨입니다.</param>
  /// <returns></returns>
  public static async Task<DataBundle> LoadBundle(string bundleName, string label) => await LoadBundle(bundleName, new AssetLabelReference{labelString = label});
  
  /// <summary>
  /// 전체 에셋을 불러오고 다시 비활성화하기 때문에 사용을 추천하지 않습니다.
  /// </summary>
  /// <param name="bundleName">불러올 DataBundle의 파일명입니다.</param>
  /// <returns></returns>
  public static async Task<DataBundle> LoadBundle(string bundleName)
  {
    AssetLabelReference label;

    if (Instance) label = Instance.bundleLabel;
    else label = new AssetLabelReference{labelString = DataBundle.DefaultLabel};
    return await LoadBundle(bundleName, label);
  }
  
  /// <summary>
  /// 전체 에셋을 불러오고 다시 비활성화하기 때문에 사용을 추천하지 않습니다.
  /// 동기적으로 패키지데이터를 불러옵니다.
  /// </summary>
  /// <param name="bundleName">불러올 DataBundle의 파일명입니다.</param>
  /// <param name="label">불러올 DataBundle의 어드레서블 라벨입니다.</param>
  /// <returns></returns>
  public static DataBundle LoadBundleSync(string bundleName, AssetLabelReference label)
  {
    var task = LoadBundle(bundleName, label);
    task.Wait();
    return task.Result;
  }
  
  /// <summary>
  /// 전체 에셋을 불러오고 다시 비활성화하기 때문에 사용을 추천하지 않습니다.
  /// 동기적으로 패키지데이터를 불러옵니다.
  /// </summary>
  /// <param name="bundleName">불러올 ScenePackage의 파일명입니다.</param>
  /// <returns></returns>
  public static DataBundle LoadBundleSync(string bundleName)
  {
    var task = LoadBundle(bundleName);
    task.Wait();
    return task.Result;
  }
  
  /// <summary>
  /// 전체 에셋을 불러오고 다시 비활성화하기 때문에 사용을 추천하지 않습니다.
  /// 동기적으로 패키지데이터를 불러옵니다.
  /// </summary>
  /// <param name="bundleName"></param>
  /// <param name="label"></param>
  /// <returns></returns>
  public static DataBundle LoadBundleSync(string bundleName, string label) => LoadBundleSync(bundleName, new AssetLabelReference{labelString = label});
  #endregion PackageLoader

  #region Unity Events

  /// <summary>
  ///   싱글톤 설정
  /// </summary>
  private void Awake()
  {
    if (Instance == null)
    {
      Instance = this;
      DontDestroyOnLoad(gameObject);
    }
    else
    {
      Destroy(gameObject);
    }
  }

  private void Start()
  {
    // ui 시작 및 로딩 중 표시
    Load();
    // 다음 씬 넘어갈 수 있게 ui 표시
  }

  #endregion Unity Events
}