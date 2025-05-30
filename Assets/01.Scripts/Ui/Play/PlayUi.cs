using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayUi : MonoBehaviour
{
    public PlaySlider health { get => fieldHealth; }
    [SerializeField] private PlaySlider fieldHealth;

    public PlaySlider stamina { get => fieldStamina; }
    [SerializeField] private PlaySlider fieldStamina;

    public PlaySlider defense { get => fieldDefense; }
    [SerializeField] private PlaySlider fieldDefense;

    public PlaySlot firstSlot { get => fieldFirstSlot; }
    [SerializeField] private PlaySlot fieldFirstSlot;

    public PlaySlot secondSlot{ get => fieldSecondSlot; }
    [SerializeField] private PlaySlot fieldSecondSlot;

    private void Reset()
    {
        var hpPos = Helper.FindChild(this.transform, "Hp");
        if (hpPos.TryGetComponent<PlaySlider>(out var isHealth)) fieldHealth = isHealth;
        else DebugHelper.ShowBugWindow($"{this.name}에 Image가 존재하지 않음");

        var staminaPos = Helper.FindChild(this.transform, "Stamina");
        if (staminaPos.TryGetComponent<PlaySlider>(out var isStamina)) fieldStamina = isStamina;
        else DebugHelper.ShowBugWindow($"{this.name}에 Image가 존재하지 않음");

        var defensePos = Helper.FindChild(this.transform, "Defense");
        if (defensePos.TryGetComponent<PlaySlider>(out var isDefense)) fieldDefense = isDefense;
        else DebugHelper.ShowBugWindow($"{this.name}에 Image가 존재하지 않음");

        var firstPos = Helper.FindChild(this.transform, "MainSlot");
        if (firstPos.TryGetComponent<PlaySlot>(out var isFirst)) fieldFirstSlot = isFirst;
        else DebugHelper.ShowBugWindow($"{this.name}에 PlaySlot가 존재하지 않음");

        var secondPos = Helper.FindChild(this.transform, "SecondSlot");
        if (secondPos.TryGetComponent<PlaySlot>(out var isSecond)) fieldSecondSlot = isSecond;
        else DebugHelper.ShowBugWindow($"{this.name}에 PlaySlot가 존재하지 않음");
    }
}
