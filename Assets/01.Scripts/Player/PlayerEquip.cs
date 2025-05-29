using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerEquip : MonoBehaviour
{
    public Dictionary<WeaponInfo, GameObject> weapons;
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

    private void Start()
    {
        AssignAnimationIDs();
        meleeCollider.enabled = false;
        
        weapons = new Dictionary<WeaponInfo, GameObject>();
        
        for (int i = 0; i < equipPivot.childCount; i++)
        {
            GameObject weapon = equipPivot.GetChild(i).gameObject;
            if (weapon.TryGetComponent(out PlayerWeapon weaponData))
            {
                var info = weaponData.GetWeaponData().Item1;
                if (info != null && !weapons.ContainsKey(info))
                {
                    weapons.Add(info, weapon);
                    weapon.SetActive(false);
                }
            }
        }
    }

    private void AssignAnimationIDs()
    {
        _animIDEquipMelee = Animator.StringToHash("EquipMelee");
        _animIDEquipRanged = Animator.StringToHash("EquipRanged");
    }

    public void EquipNew(WeaponInfo data)
    {
        if (!weapons.TryGetValue(data, out GameObject weapon))
        {
            Debug.Log("해당 무기를 찾을 수 없습니다.");
            return;
        }

        if (!weapon.TryGetComponent(out PlayerWeapon weaponData))
        {
            Debug.Log("무기 컴포넌트를 찾을 수 없습니다.");
            return;
        }

        var (info, type) = weaponData.GetWeaponData();
        if (info == null || info.itemId != data.itemId)
        {
            Debug.Log("무기 정보가 일치하지 않습니다.");
            return;
        }

        if (curWeapon != null)
        {
            if (curWeapon.transform.parent != equipPivot)
                return;

            if (curWeapon == weapon)
            {
                Unequip();
                return;
            }

            Unequip();
        }

        switch (type)
        {
            case PlayerWeaponType.Melee:
                _equipMelee = true;
                break;
            case PlayerWeaponType.Ranged:
                _equipRanged = true;
                break;
        }

        curEquip = data;
        curWeapon = weapon;
        curWeaponData = weaponData;
        curWeapon.SetActive(true);
        curWeaponType = type;

        UpdateAnimationBools();
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
