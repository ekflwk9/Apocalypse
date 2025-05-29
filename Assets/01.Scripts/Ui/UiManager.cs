using UnityEngine;

public class UiManager : MonoBehaviour
{
    public static UiManager instance { get; private set; }

    public bool isActive { get; private set; }

    public MenuUi menu { get => fieldMenu; }
    [SerializeField] private MenuUi fieldMenu;

    public PlayUi play { get => fieldPlay; }
    [SerializeField] private PlayUi fieldPlay;

    public StatusUi status { get => fieldStatus; }
    [SerializeField] private StatusUi fieldStatus;

    public ShaderUi shader { get => fieldShader; }
    [SerializeField] private ShaderUi fieldShader;

    public Fade fade { get => fieldFade; }
    [SerializeField] private Fade fieldFade;

    public TouchUi touch { get => fieldTouch; }
    [SerializeField] private TouchUi fieldTouch;

    private void Reset()
    {
        var playPos = Helper.FindChild(this.transform, "PlayUi");
        if(playPos.TryGetComponent<PlayUi>(out var isPlay)) fieldPlay = isPlay;
        else DebugHelper.Log($"{this.name}에 PlayUi가 존재하지 않음");

        var menuPos = Helper.FindChild(this.transform, "MenuUi").gameObject;
        if (menuPos.TryGetComponent<MenuUi>(out var isMenu)) fieldMenu = isMenu;
        else DebugHelper.Log($"{this.name}에 MenuUi가 존재하지 않음");

        fieldStatus = this.GetComponentInChildren<StatusUi>(true);
        if (fieldStatus == null) DebugHelper.Log($"{this.name}에 StatusUi스크립트가 있는 자식 오브젝트가 존재하지 않음");

        fieldShader = this.GetComponentInChildren<ShaderUi>(true);
        if (fieldShader == null) DebugHelper.Log($"{this.name}에 ShaderUi스크립트가 있는 자식 오브젝트가 존재하지 않음");

        fieldTouch = this.GetComponentInChildren<TouchUi>(true);
        if (fieldTouch == null) DebugHelper.Log($"{this.name}에 TouchUi스크립트가 있는 자식 오브젝트가 존재하지 않음");

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

    public void SetActive(bool _isActive) => isActive = _isActive;
}
