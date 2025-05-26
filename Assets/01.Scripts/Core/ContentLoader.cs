using UnityEngine;
using UnityEngine.AddressableAssets;

public class ContentLoader : MonoBehaviour
{
  private GameManager gameManager => GameManager.Instance;
  
  [SerializeField] private AssetLabelReference prefabLabel;

  private void Start()
  {
    Load();
  }

  /// <summary>
  /// 로딩하고 싶은 데이터가 있을 시 여기에 추가
  /// </summary>
  private void Load()
  {
    
  }
}