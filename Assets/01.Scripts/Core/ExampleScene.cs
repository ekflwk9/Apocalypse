using System.Threading.Tasks;

public class ExampleScene : SceneBundle
{
  protected override async Task PreLoad(SerializableDictionary<string, DataSet> bundles)
  {
  }

  protected override void Ready()
  {
  }

  protected override async Task UnLoad()
  {
    await base.UnLoad();
  }
}