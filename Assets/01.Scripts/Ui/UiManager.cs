using UnityEngine;

public class UiManager : MonoBehaviour
{
    public static UiManager instance { get; private set; }

    public MenuUi menu { get => fieldMenu; }
    [SerializeField] private MenuUi fieldMenu;

    public StatusUi status { get => fieldStatus; }
    [SerializeField] private StatusUi fieldStatus;

    public ShaderUi shader { get => fieldShader; }
    [SerializeField] private ShaderUi fieldShader;

    public Fade fade { get => fieldFade; }
    [SerializeField] private Fade fieldFade;

    private void Reset()
    {
        //true = ��Ȱ��ȭ ������Ʈ�� �˻�
        fieldMenu = this.GetComponentInChildren<MenuUi>(true);
        if (fieldMenu == null) DebugHelper.Log($"{this.name}�� MenuUi��ũ��Ʈ�� �ִ� �ڽ� ������Ʈ�� �������� ����");

        fieldStatus = this.GetComponentInChildren<StatusUi>(true);
        if (fieldStatus == null) DebugHelper.Log($"{this.name}�� StatusUi��ũ��Ʈ�� �ִ� �ڽ� ������Ʈ�� �������� ����");

        fieldShader = this.GetComponentInChildren<ShaderUi>(true);
        if (fieldShader == null) DebugHelper.Log($"{this.name}�� ShaderUi��ũ��Ʈ�� �ִ� �ڽ� ������Ʈ�� �������� ����");

        fieldFade = this.GetComponentInChildren<Fade>(true);
        if (fieldFade == null) DebugHelper.Log($"{this.name}�� Fade��ũ��Ʈ�� �ִ� �ڽ� ������Ʈ�� �������� ����");
    }

    private void Awake()
    {
        if (UiManager.instance == null)
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
