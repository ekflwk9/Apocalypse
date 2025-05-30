using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayUi : MonoBehaviour
{
    public Image health { get => fieldHealth; }
    [SerializeField] private Image fieldHealth;

    public Image stamina { get => fieldStamina; }
    [SerializeField] private Image fieldStamina;

    public Image defense { get => fieldDefense; }
    [SerializeField] private Image fieldDefense;

    public PlaySlot firstSlot { get => fieldFirstSlot; }
    [SerializeField] private PlaySlot fieldFirstSlot;

    public PlaySlot secondSlot{ get => fieldSecondSlot; }
    [SerializeField] private PlaySlot fieldSecondSlot;

    private void Reset()
    {
        var hpPos = Helper.FindChild(this.transform, "Hp");
        if (hpPos.TryGetComponent<Image>(out var isHealth)) fieldHealth = isHealth;
        else DebugHelper.ShowBugWindow($"{this.name}에 Image가 존재하지 않음");

        var staminaPos = Helper.FindChild(this.transform, "Stamina");
        if (staminaPos.TryGetComponent<Image>(out var isStamina)) fieldStamina = isStamina;
        else DebugHelper.ShowBugWindow($"{this.name}에 Image가 존재하지 않음");

        var defensePos = Helper.FindChild(this.transform, "Defense");
        if (defensePos.TryGetComponent<Image>(out var isDefense)) fieldDefense = isDefense;
        else DebugHelper.ShowBugWindow($"{this.name}에 Image가 존재하지 않음");

        var firstPos = Helper.FindChild(this.transform, "MainSlot");
        if (firstPos.TryGetComponent<PlaySlot>(out var isFirst)) fieldFirstSlot = isFirst;
        else DebugHelper.ShowBugWindow($"{this.name}에 PlaySlot가 존재하지 않음");

        var secondPos = Helper.FindChild(this.transform, "SecondSlot");
        if (secondPos.TryGetComponent<PlaySlot>(out var isSecond)) fieldSecondSlot = isSecond;
        else DebugHelper.ShowBugWindow($"{this.name}에 PlaySlot가 존재하지 않음");
    }
}
