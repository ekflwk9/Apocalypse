using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardWindow : MonoBehaviour
{
    [Header("플레이어 레벨 등급 종류")]
    [SerializeField] private string[] levelName;

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
        info.text = levelName[index];
    }
}
