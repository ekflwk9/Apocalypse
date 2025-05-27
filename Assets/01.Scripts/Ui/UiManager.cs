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
        //true = 비활성화 오브젝트도 검색
        fieldMenu = this.GetComponentInChildren<MenuUi>(true);
        if (fieldMenu == null) DebugHelper.Log($"{this.name}에 MenuUi스크립트가 있는 자식 오브젝트가 존재하지 않음");

        fieldStatus = this.GetComponentInChildren<StatusUi>(true);
        if (fieldStatus == null) DebugHelper.Log($"{this.name}에 StatusUi스크립트가 있는 자식 오브젝트가 존재하지 않음");

        fieldShader = this.GetComponentInChildren<ShaderUi>(true);
        if (fieldShader == null) DebugHelper.Log($"{this.name}에 ShaderUi스크립트가 있는 자식 오브젝트가 존재하지 않음");

        fieldFade = this.GetComponentInChildren<Fade>(true);
        if (fieldFade == null) DebugHelper.Log($"{this.name}에 Fade스크립트가 있는 자식 오브젝트가 존재하지 않음");
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
