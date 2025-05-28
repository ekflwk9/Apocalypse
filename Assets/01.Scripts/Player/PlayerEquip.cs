using System;
using GameItem;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerEquip : MonoBehaviour
{
    public GameObject[] Weapons;
    public WeaponInfo curEquip;
    [SerializeField] private Transform equipPivot;
    [SerializeField] private Transform meleeWeaponPivot;
    [SerializeField] private Transform rangedWeaponPivot;
    private Transform handPosition;
    
    public BoxCollider meleeCollider;
    
    [SerializeField] private PlayerInputs _input;
    [SerializeField] private Animator _animator;
    private int _animIDEquipMelee;
    private int _animIDEquipRanged;
    private bool _equipMelee;
    private bool _equipRanged;
    private bool _isWeaponOnHand = false;
    [SerializeField] private bool _toggleMelee = false;
    
    //테스트 코드
    public GameObject SelectWeapon;
    //

    private void Reset()
    {
        _animator = GetComponent<Animator>();
        _input = GetComponent<PlayerInputs>();
    }

    private void Start()
    {
        AssignAnimationIDs();
        meleeCollider.enabled = false;
        Weapons = new GameObject[equipPivot.childCount];
        for (int i = 0; i < equipPivot.childCount; i++)
        {
            Weapons[i] = equipPivot.GetChild(i).gameObject;
            Weapons[i].SetActive(false);
        }
    }

    private void AssignAnimationIDs()
    {
        _animIDEquipMelee = Animator.StringToHash("EquipMelee");
        _animIDEquipRanged = Animator.StringToHash("EquipRanged");
    }

    public void EquipNew(WeaponInfo data)
    {
        foreach (var weapon in Weapons)
        {
            PlayerWeapon weaponsData;
            WeaponInfo weaponInfo;
            bool hasWeaponData = weapon.TryGetComponent<PlayerWeapon>(out weaponsData);
            if (!hasWeaponData)
            {
                Debug.Log("무기 정보 없음");
            }
            else
            {
                weaponInfo = weaponsData.GetWeaponData();
                if (weaponInfo == null || weaponInfo.itemId != data.itemId) continue;
                
                if(SelectWeapon != null)
                {
                    if (SelectWeapon.transform.parent != equipPivot)
                    {
                        return;
                    }
            
                    if (SelectWeapon == weapon)
                    {
                        Unequip();
                        return;
                    }
            
                    Unequip();
                }
                
                curEquip = data;
                SelectWeapon = weapon;
                SelectWeapon.SetActive(true);
                
                //아래 각 조건은 근접, 원거리 무기를 구분하도록 변경해야함.
                if (0 < curEquip.itemId && curEquip.itemId <= 50)
                {
                    _equipMelee = true;
                }
                else if (50 < curEquip.itemId && curEquip.itemId < 100)
                {
                    _equipRanged = true;
                }
                //
                _animator.SetBool(_animIDEquipMelee, _equipMelee);
                _animator.SetBool(_animIDEquipRanged, _equipRanged);
            }
        }
    }
    
    private void Unequip()
    {
        curEquip = null;
        SelectWeapon.SetActive(false);
        SelectWeapon = null;
        _equipMelee = false;
        _equipRanged = false;
    }

    public void ToggleWeaponLocation()
    {
        _isWeaponOnHand = !_isWeaponOnHand; 
        
        if (_equipMelee)
        {
            handPosition = meleeWeaponPivot;
        }
        else if (_equipRanged)
        {
            handPosition = rangedWeaponPivot;
        }
        
        SelectWeapon.transform.SetParent(_isWeaponOnHand ? handPosition : equipPivot, false);
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
