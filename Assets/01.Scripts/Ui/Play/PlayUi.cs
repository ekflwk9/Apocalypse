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
        fieldHealth = this.TryFindChildComponent<PlaySlider>("Hp");
        fieldStamina = this.TryFindChildComponent<PlaySlider>("Stamina");
        fieldDefense = this.TryFindChildComponent<PlaySlider>("Defense");
        fieldFirstSlot = this.TryFindChildComponent<PlaySlot>("MainSlot");
        fieldSecondSlot = this.TryFindChildComponent<PlaySlot>("SecondSlot");
    }
}
