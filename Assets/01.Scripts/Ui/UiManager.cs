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
            //true = 비활성화 오브젝트도 검색
            menu = this.GetComponentInChildren<MenuUi>(true); 
            if (menu == null) DebugHelper.Log($"{this.name}에 MenuUi스크립트가 있는 자식 오브젝트가 존재하지 않음");

            status = this.GetComponentInChildren<StatusUi>(true);
            if (status == null) DebugHelper.Log($"{this.name}에 StatusUi스크립트가 있는 자식 오브젝트가 존재하지 않음");

            drag = this.GetComponentInChildren<DragImage>(true);
            if (drag == null) DebugHelper.Log($"{this.name}에 DragImage스크립트가 있는 자식 오브젝트가 존재하지 않음");

            fade = this.GetComponentInChildren<Fade>(true);
            if (fade == null) DebugHelper.Log($"{this.name}에 Fade스크립트가 있는 자식 오브젝트가 존재하지 않음");

            shader = this.GetComponentInChildren<ShaderUi>(true);
            if (shader == null) DebugHelper.Log($"{this.name}에 ShaderUi스크립트가 있는 자식 오브젝트가 존재하지 않음");

            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        else
        {
            Destroy(this.gameObject);
        }
    }
}
