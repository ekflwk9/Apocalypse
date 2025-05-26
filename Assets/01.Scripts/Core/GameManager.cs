using UnityEngine;

public class GameManager : MonoBehaviour
{
  public static GameManager Instance { get; private set; }

  public SerializableDictionary<string, Sprite> sprites = new();
  public SerializableDictionary<string, AudioClip> audioClips = new();
  public SerializableDictionary<string, GameObject> prefabs = new();

  private void Awake()
  {
    if(Instance == null)
    {
      Instance = this;
      DontDestroyOnLoad(gameObject);
    }
    else
    {
      Destroy(gameObject);
    }
  }
}
