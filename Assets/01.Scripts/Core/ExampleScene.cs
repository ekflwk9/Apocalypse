using System.Collections;
using GameItem;

public class ExampleScene : SceneBundle
{
  protected override IEnumerator PreLoad(SerializableDictionary<string, AssetData> loadedAssets)
  {
    var items = loadedAssets["items"];
    var sprite = items.Sprites[""];

    if (CurrentBundle is ExampleScene scene)
    {
      
    }
    
    yield return null;
  }

  protected override void Ready()
  {
  }

  protected override void UnLoad()
  {
    base.UnLoad();
  }
}