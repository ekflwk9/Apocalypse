using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "new ScenePackage", menuName = "Apocalypse/ScenePackage", order = 0)]
public class ScenePackage : ScriptableObject
{
  [SerializeField] private AssetReference[] assetReferences;
  [SerializeField] private AssetLabelReference[] assetLabelReferences;

  public SceneData Load()
  {
    var loadedAssets = new List<object>();

    // 에셋을 동기로 불러오고 null 아닐시 메모리에 올림
    foreach (var reference in assetReferences)
    {
      var asset = reference.LoadAssetAsync<object>().WaitForCompletion();
      
      if (asset != null)
        loadedAssets.Add(asset);
    }

    // 라벨 붙은 에셋들을 동기로 불러오고 null이 아닐시 메모리에 올림
    foreach (var label in assetLabelReferences)
    {
      Addressables.LoadAssetsAsync<object>(label, obj =>
      {
        if (obj != null) loadedAssets.Add(obj);
      }).WaitForCompletion();
    }

    return new SceneData(loadedAssets);
  }
}