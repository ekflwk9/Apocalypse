using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    [Header("Character Input Values")]
    public Vector2 move;
    public Vector2 look;

    [Header("Movement Settings")]
    public bool analogMovement;

    [Header("Mouse Cursor Settings")]
    public bool cursorLocked = true;

    [SerializeField] private Player _player;
    [SerializeField] private PlayerThirdPersonController _controller;
    [SerializeField] private PlayerInteraction _playerInteraction;

    private bool _canSprint = true;
    public bool Jump { get; private set; }

    public bool Sprint { get; private set; }

    public bool Aim { get; private set; }

    public bool Attack { get; private set; }

    private void Reset()
    {
        _player = GetComponent<Player>();
        _controller = GetComponent<PlayerThirdPersonController>();
        _playerInteraction = GetComponentInChildren<PlayerInteraction>();
    }

    private void Update()
    {
        if (Sprint)
        {
            _player.SetStamina(-(Time.deltaTime * _player.sprintStamina));

            if (_player.Stamina <= 0f)
            {
                Sprint = false;
                _canSprint = false;
            }
        }
        else if (!_canSprint && _player.Stamina > _player.maxStamina / 4f)
        {
            _canSprint = true;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        look = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            if (_player.Stamina < _player.jumpStamina)
            {
                return;
            }

            _player.SetStamina(-(_player.jumpStamina));
            Jump = true;
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            Jump = false;
        }
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (!_canSprint)
        {
            return;
        }

        Sprint = context.ReadValueAsButton();
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        Aim = context.ReadValueAsButton();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            Attack = true;

            switch (Player.Instance.Equip.curWeaponType)
            {
                case PlayerWeaponType.Melee:
                    _controller.MeleeAttack();
                    Attack = false;
                    break;
                case PlayerWeaponType.Ranged:
                    _controller.RangedAttack();
                    Attack = false;
                    break;
                case PlayerWeaponType.Consumable:
                    _controller.UseItem();
                    Attack = false;
                    break;
            }
        }
    }

    public void OnNumberInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            float key = context.ReadValue<float>();
            int numberPressed = Mathf.RoundToInt(key);
            switch (numberPressed)
            {
                case 1:
                    ItemInfo firstItem = ItemManager.Instance.Inventory.firstSlotItem;
                    if (firstItem == null)
                    {
                        return;
                    }

                    Player.Instance.Equip.EquipItem(ItemManager.Instance.GetItem(firstItem.itemId));
                    break;
                case 2:
                    ItemInfo secondItem = ItemManager.Instance.Inventory.secondSlotItem;
                    if (secondItem == null)
                    {
                        return;
                    }

                    Player.Instance.Equip.EquipItem(ItemManager.Instance.GetItem(secondItem.itemId));
                    break;
            }
        }
    }
    
    public void OnInteraction(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            _playerInteraction.Interaction();
            Player.Instance.Sound.LootSound();
        }
    }
}