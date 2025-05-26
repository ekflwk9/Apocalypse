using UnityEngine;

public enum UiCode
{
    None = 0,
    Inventory = 1,
    Storage = 2,
    PlayerInfo = 3,
    Farming = 4,
}

public class UiManager : MonoBehaviour
{
    public static UiManager instance { get; private set; }
    public MenuUi menu { get; private set; }
    public StatusUi status { get; private set; }
    public DragImage drag { get; private set; }
    public ShaderUi shader { get; private set; }
    public Fade fade { get; private set; }

    private void Awake()
    {
        if (UiManager.instance == null)
        {
            //true = ��Ȱ��ȭ ������Ʈ�� �˻�
            menu = this.GetComponentInChildren<MenuUi>(true); 
            if (menu == null) DebugHelper.Log($"{this.name}�� MenuUi��ũ��Ʈ�� �ִ� �ڽ� ������Ʈ�� �������� ����");

            status = this.GetComponentInChildren<StatusUi>(true);
            if (status == null) DebugHelper.Log($"{this.name}�� StatusUi��ũ��Ʈ�� �ִ� �ڽ� ������Ʈ�� �������� ����");

            drag = this.GetComponentInChildren<DragImage>(true);
            if (drag == null) DebugHelper.Log($"{this.name}�� DragImage��ũ��Ʈ�� �ִ� �ڽ� ������Ʈ�� �������� ����");

            fade = this.GetComponentInChildren<Fade>(true);
            if (fade == null) DebugHelper.Log($"{this.name}�� Fade��ũ��Ʈ�� �ִ� �ڽ� ������Ʈ�� �������� ����");

            shader = this.GetComponentInChildren<ShaderUi>(true);
            if (shader == null) DebugHelper.Log($"{this.name}�� ShaderUi��ũ��Ʈ�� �ִ� �ڽ� ������Ʈ�� �������� ����");

            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        else
        {
            Destroy(this.gameObject);
        }
    }
}
