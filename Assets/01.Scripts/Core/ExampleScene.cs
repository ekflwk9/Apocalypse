using System.Collections;

public class ExampleScene : SceneBundle
{
  protected override IEnumerator OnScenePreLoad(SerializableDictionary<string, AssetData> loadedAssets)
  {
    var items = loadedAssets["items"];
    var sprite = items.Sprites[""];

    if (CurrentBundle is ExampleScene scene)
    {
      
    }
    
    yield return null;
  }

  protected override void OnSceneStart()
  {
  }

  protected override void OnSceneEnd()
  {
    base.OnSceneEnd();
  }
}