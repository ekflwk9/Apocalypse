using UnityEngine;

public class UiManager : MonoBehaviour
{
    public static UiManager instance { get; private set; }

    public bool isActive { get; private set; } = true;

    public HitShader hitUi { get => fieldHitUi; }
    [SerializeField] private HitShader fieldHitUi;

    public MenuUi menu { get => fieldMenu; }
    [SerializeField] private MenuUi fieldMenu;

    public PlayUi play { get => fieldPlay; }
    [SerializeField] private PlayUi fieldPlay;

    public StatusUi status { get => fieldStatus; }
    [SerializeField] private StatusUi fieldStatus;

    public UiShaderEffect shaderEffect { get => fieldShaderEffect; }
    [SerializeField] private UiShaderEffect fieldShaderEffect;

    public Fade fade { get => fieldFade; }
    [SerializeField] private Fade fieldFade;

    public TouchUi touch { get => fieldTouch; }
    [SerializeField] private TouchUi fieldTouch;

    public LobyUi lobyUi { get => fieldLobyUi; }
    [SerializeField] private LobyUi fieldLobyUi;

    public InterfaceUi interactionUi { get => fieldInteractionUi; }
    [SerializeField] private InterfaceUi fieldInteractionUi;

    private void Reset()
    {
        fieldPlay = this.TryFindChildComponent<PlayUi>("PlayUi");
        fieldMenu = this.TryFindChildComponent<MenuUi>("MenuUi");
        fieldHitUi= this.TryFindChildComponent<HitShader>("HitVolume");
        fieldStatus = this.TryFindChildComponent<StatusUi>();
        fieldShaderEffect = this.TryFindChildComponent<UiShaderEffect>();
        fieldTouch = this.TryFindChildComponent<TouchUi>();
        fieldFade = this.TryFindChildComponent<Fade>();
        fieldLobyUi = this.TryFindChildComponent<LobyUi>();
        fieldInteractionUi = this.TryFindChildComponent<InterfaceUi>();
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

    public void Start() => fade.gameObject.SetActive(true);
    
    public void SetActive(bool _isActive) => isActive = _isActive;
}
