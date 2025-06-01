using UnityEngine;

public class InterfaceUi : MonoBehaviour
{
    public GameObject playButton { get => fieldPlayButton; }
    [SerializeField] private GameObject fieldPlayButton;
    public GameObject settingButton { get => fieldSettingButton; }
    [SerializeField] private GameObject fieldSettingButton;

    public GameObject backButton { get => fieldBackButton; }
    [SerializeField] private GameObject fieldBackButton;

    public GameObject storageButton { get => fieldstorageButton; }
    [SerializeField] private GameObject fieldstorageButton;

    public GoldWindow gold { get => fieldGold; }
    [SerializeField] private GoldWindow fieldGold;

    public NoMoneyWindow noMoney { get => fieldNoMoney; }
    [SerializeField] private NoMoneyWindow fieldNoMoney;

    private void Reset()
    {
        fieldPlayButton = this.TryFindChild("PlayButton").gameObject;
        fieldSettingButton = this.TryFindChild("SettingButton").gameObject;
        fieldBackButton = this.TryFindChild("BackButton").gameObject;
        fieldstorageButton = this.TryFindChild("StorageButton").gameObject;
        fieldGold = this.TryFindChildComponent<GoldWindow>();
        fieldNoMoney = this.TryFindChildComponent<NoMoneyWindow>();
    }

    /// <summary>
    /// 기본 Ui창은 true
    /// </summary>
    /// <param name="_isActive"></param>
    public void SwitchBackButton(bool _isActive)
    {
        playButton.SetActive(_isActive);
        settingButton.SetActive(_isActive);
        fieldstorageButton.SetActive(_isActive);
        UiManager.instance.lobyUi.title.SetActive(_isActive);

        backButton.SetActive(!_isActive);
    }
}
