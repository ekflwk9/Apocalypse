using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerEquip : MonoBehaviour
{
    public GameObject[] weapons;
    public GameObject curWeapon;
    public PlayerWeapon curWeaponData;
    
    public WeaponInfo curEquip;
    public PlayerWeaponType curWeaponType = PlayerWeaponType.None;
    
    [SerializeField] private Transform equipPivot;
    [SerializeField] private Transform weaponPivot;
    
    public BoxCollider meleeCollider;
    
    [SerializeField] private PlayerInputs _input;
    [SerializeField] private Animator _animator;
    private int _animIDEquipMelee;
    private int _animIDEquipRanged;
    private bool _equipMelee;
    private bool _equipRanged;
    private bool _isWeaponOnHand = false;
    [SerializeField] private bool _toggleMelee = false;

    private void Reset()
    {
        _animator = GetComponent<Animator>();
        _input = GetComponent<PlayerInputs>();
    }

    private void Start()
    {
        AssignAnimationIDs();
        meleeCollider.enabled = false;
        weapons = new GameObject[equipPivot.childCount];
        for (int i = 0; i < equipPivot.childCount; i++)
        {
            weapons[i] = equipPivot.GetChild(i).gameObject;
            weapons[i].SetActive(false);
        }
    }

    private void AssignAnimationIDs()
    {
        _animIDEquipMelee = Animator.StringToHash("EquipMelee");
        _animIDEquipRanged = Animator.StringToHash("EquipRanged");
    }

    public void EquipNew(WeaponInfo data)
    {
        WeaponInfo _weaponInfo;
        PlayerWeaponType _weaponType;
        
        foreach (var weapon in weapons)
        {
            if (!weapon.TryGetComponent(out curWeaponData))
            {
                Debug.Log("무기 정보 없음");
            }
            else
            {
                (_weaponInfo, _weaponType) = curWeaponData.GetWeaponData();
                if (_weaponInfo == null || _weaponInfo.itemId != data.itemId) continue;
                
                if(curWeapon != null)
                {
                    if (curWeapon.transform.parent != equipPivot)
                    {
                        return;
                    }
            
                    if (curWeapon == weapon)
                    {
                        Unequip();
                        return;
                    }
            
                    Unequip();
                }
                
                switch (_weaponType)
                {
                    case PlayerWeaponType.Melee:
                        _equipMelee = true;
                        break;
                    case PlayerWeaponType.Ranged:
                    case PlayerWeaponType.RangedAuto:
                        _equipRanged = true;
                        break;
                    default:
                        break;
                }
                
                                
                curEquip = data;
                curWeapon = weapon;
                curWeapon.SetActive(true);
                curWeaponType = _weaponType;

                UpdateAnimationBools();
            }
        }
    }
    
    private void Unequip()
    {
        curEquip = null;
        
        if (curWeapon != null)
        {
            curWeapon.SetActive(false);
            curWeapon = null;
        }
        
        curWeaponType = PlayerWeaponType.None;
        _equipMelee = false;
        _equipRanged = false;

        UpdateAnimationBools();
    }

    private void UpdateAnimationBools()
    {
        _animator.SetBool(_animIDEquipMelee, _equipMelee);
        _animator.SetBool(_animIDEquipRanged, _equipRanged);
    }

    public void ToggleWeaponLocation()
    {
        _isWeaponOnHand = !_isWeaponOnHand; 
        
        curWeapon.transform.SetParent(_isWeaponOnHand ? weaponPivot : equipPivot, false);

        (Vector3 position, Vector3 rotation) = curWeaponData.GetEquipPosition(_isWeaponOnHand);
        curWeapon.transform.localPosition = position;
        curWeapon.transform.localRotation = Quaternion.Euler(rotation);
    }
    
    public void ToggleMeleeCollider()
    {
        if (_equipMelee)
        {
            _toggleMelee = !_toggleMelee;
            meleeCollider.enabled = _toggleMelee;
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        //if (other.gameObject.layer != LayerMask.NameToLayer("Enemy")) return;
        IDamagable damagable;
        bool isEnemy = other.TryGetComponent(out damagable);
        if (!isEnemy)
        {
            return;
        }
        else
        {
            Debug.Log($"{other.gameObject.name} 맞았다.");
            damagable.TakeDamage(curEquip != null ? curEquip.power : 10f);
        }
    }
}
