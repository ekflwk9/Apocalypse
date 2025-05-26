using UnityEngine;

public interface IUiActiveHandler { public void SetActiveUi(); }

public class UiController : MonoBehaviour
{
    private IUiActiveHandler[] active;
    public static UiController instance { get; private set; }

    private void Reset()
    {
        if (active.Length == 0)
        {
            var childCount = this.transform.childCount;

            if (childCount == 0)
            {
                Debug.Log("해당 오브젝트의 자식이 존재하지 않음");
                return;
            }

            active = new IUiActiveHandler[childCount];

            for (int i = 0; i < childCount; i++)
            {

            }
        }
    }

    private void Awake()
    {
        if (UiController.instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        else
        {
            Destroy(this.gameObject);
        }
    }
}
