using TMPro;
using UnityEngine;

public class CardWindow : MonoBehaviour
{
    [Header("플레이어 레벨 등급 종류")]
    [SerializeField] private string[] levelName;
    [SerializeField] private int[] cost;

    [Space(10f)]
    [SerializeField] private Animator anim;
    [SerializeField] private TMP_Text info;

    private void Reset()
    {
        anim = this.TryGetComponent<Animator>();
        info = this.TryFindChildComponent<TMP_Text>("RankText");
    }

    public void UpdateCard()
    {
        anim.Play("Idle", 0, 0);
        var index = Player.Instance.Level;

        if (index < levelName.Length)
        {
            info.text = levelName[index];
            UiManager.instance.lobyUi.SetNeedGoldText($"{cost[index].ToString("N0")}만원");
        }
    }

    public int GetCost()
    {
        if (Player.Instance.Level >= cost.Length || cost.Length == 0)
        {
            return cost.Length - 1;
        }

        else
        {
            return cost[Player.Instance.Level];
        }
    }
}
