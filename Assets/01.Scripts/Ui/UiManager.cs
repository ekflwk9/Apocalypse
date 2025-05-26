using UnityEngine;

public enum UiCode
{
    None = 0,
    Inventory = 1,
    Storage = 1,
    PlayerInfo = 1,
    Farming = 1,
}

public class UiManager : MonoBehaviour
{
    public static UiManager instance { get; private set; }
    public MenuUi menu { get; private set; }
    public StatusUi status { get; private set; }
    public DragImage drag { get; private set; }

    private void Awake()
    {
        if (UiManager.instance == null)
        {
            menu = this.GetComponentInChildren<MenuUi>();
            if (menu == null) DebugHelper.Log($"{this.name}�� MenuUi��ũ��Ʈ�� �ִ� �ڽ� ������Ʈ�� �������� ����");

            status = this.GetComponentInChildren<StatusUi>();
            if (status == null) DebugHelper.Log($"{this.name}�� StatusUi��ũ��Ʈ�� �ִ� �ڽ� ������Ʈ�� �������� ����");

            drag = this.GetComponentInChildren<DragImage>();
            if (drag == null) DebugHelper.Log($"{this.name}�� DragImage��ũ��Ʈ�� �ִ� �ڽ� ������Ʈ�� �������� ����");

            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        else
        {
            Destroy(this.gameObject);
        }
    }
}
