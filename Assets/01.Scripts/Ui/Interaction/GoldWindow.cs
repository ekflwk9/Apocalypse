using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GoldWindow : MonoBehaviour
{
    [SerializeField] TMP_Text goldText;

    private void Reset()
    {
        goldText = this.TryFindChildComponent<TMP_Text>("GoldText");
    }

    private void Start()
    {
        UpdateGold();
    }

    public void UpdateGold()
    {
        goldText.text = Player.Instance.Gold.ToString("N0");
    }
}
