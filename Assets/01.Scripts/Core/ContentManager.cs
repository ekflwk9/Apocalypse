using UnityEngine;
using UnityEngine.AddressableAssets;

public sealed class ContentManager : MonoBehaviour
{
  /// <summary>
  ///   게임중 상시 메모리에 올라가있는 데이터
  /// </summary>
  public SerializableDictionary<string, GameObject> prefabs = new();

  public SerializableDictionary<string, AudioClip> audioClips = new();
  public SerializableDictionary<string, Sprite> sprites = new();

  // 프로젝트에서 영구적으로 메모리상에 올려둘 에셋 목록
  public AssetLabelReference sharedLabel;
  public static ContentManager Instance { get; private set; }


  /// <summary>
  ///   로딩하고 싶은 데이터가 있을 시 여기에 추가
  /// </summary>
  private void Load()
  {
    Addressables.LoadAssetsAsync<object>(sharedLabel, obj =>
    {
      switch (obj)
      {
        case AudioClip clip:
          audioClips[clip.name] = clip;
          break;

        case Sprite sprite:
          sprites[sprite.name] = sprite;
          break;

        case GameObject prefab:
          prefabs[prefab.name] = prefab;
          break;
      }
    });
  }

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