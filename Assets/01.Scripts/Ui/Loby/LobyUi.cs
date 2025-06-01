using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LobyUi : MonoBehaviour
{
    public GameObject title { get => fieldTitle; }
    [SerializeField] private GameObject fieldTitle;

    public CardWindow card { get => fieldCard; }
    [SerializeField] private CardWindow fieldCard;

    public GameObject levelUpWindow { get => fieldLevelUpWindow; }
    [SerializeField] private GameObject fieldLevelUpWindow;
    [SerializeField] private TMP_Text gold;

    public Dictionary<int, GameObject> lockWindow = new Dictionary<int, GameObject>();

    private void Reset()
    {
        fieldTitle = this.TryFindChild("LobyTitle").gameObject;
        fieldLevelUpWindow = this.TryFindChild("LevelUp").gameObject;
        gold = this.TryFindChildComponent<TMP_Text>("GoldText");
        fieldCard = this.TryFindChildComponent<CardWindow>();
    }

    private void Start()
    {
        FindLockWindow(this.transform, "LockWindow");
        UpdateLockWindow();
    }

    private void FindLockWindow(Transform _parent, string _childName)
    {
        var findCount = 0;

        for (int i = 0; i < _parent.childCount; i++)
        {
            var child = _parent.GetChild(i);

            if(child.name.Contains(_childName))
            {
                findCount++;
                lockWindow.Add(findCount, child.gameObject);
            }

            else
            {
                FindLockWindow(child, _childName);
            }
        }
    }

    public void SetNeedGoldText(string _text)
    {
        gold.text = _text;
    }

    public void UpdateLockWindow()
    {
        var level = Player.Instance.Level;

        if (lockWindow.ContainsKey(level)) lockWindow[level].SetActive(false);
        //else DebugHelper.Log($"{level}번의 lockWindow가 존재하지 않음");
    }
}
