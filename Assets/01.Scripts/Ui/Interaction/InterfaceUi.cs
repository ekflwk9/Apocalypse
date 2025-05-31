using UnityEngine;

public class InterfaceUi : MonoBehaviour
{
    public GameObject playButton { get => fieldPlayButton; }
    [SerializeField] private GameObject fieldPlayButton;
    public GameObject settingButton { get => fieldSettingButton; }
    [SerializeField] private GameObject fieldSettingButton;

    public GameObject backButton { get => fieldBackButton; }
    [SerializeField] private GameObject fieldBackButton;

    private void Reset()
    {
        fieldPlayButton = this.TryFindChild("PlayButton").gameObject;
        fieldSettingButton = this.TryFindChild("SettingButton").gameObject;
        fieldBackButton = this.TryFindChild("BackButton").gameObject;
    }
}
