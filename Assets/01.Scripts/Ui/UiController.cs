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
                Debug.Log("�ش� ������Ʈ�� �ڽ��� �������� ����");
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
