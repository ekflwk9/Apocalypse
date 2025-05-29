using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerEquip : MonoBehaviour
{
    private Dictionary<ItemInfo, GameObject> weaponPrefabs;
    
    public GameObject curWeaponPrefab;
    public PlayerWeapon curWeaponData;
    
    public ItemInfo curEquip;
    public WeaponInfo curWeapon;
    
    public PlayerWeaponType curWeaponType = PlayerWeaponType.None;
    
    [SerializeField] private Transform equipPivot;
    [SerializeField] private Transform weaponPivot;
    
    public BoxCollider meleeCollider;
    
    [SerializeField] private PlayerInputs _input;
    [SerializeField] private Animator _animator;
    private int _animIDEquipWeapon;
    private int _animIDEquipMelee;
    private int _animIDEquipRanged;
    private int _animIDEquipItem;
    private int _animIDUnEquip;
    private bool _equipMelee;
    private bool _equipRanged;
    private bool _equipItem;
    private bool _isWeaponOnHand = false;
    [SerializeField] private bool _toggleMelee = false;

    private void Start()
    {
        AssignAnimationIDs();
        meleeCollider.enabled = false;
        
        weaponPrefabs = new Dictionary<ItemInfo, GameObject>();
        
        for (int i = 0; i < equipPivot.childCount; i++)
        {
            GameObject weapon = equipPivot.GetChild(i).gameObject;
            if (weapon.TryGetComponent(out PlayerWeapon weaponData))
            {
                var info = weaponData.GetItemData();
                if (info != null && !weaponPrefabs.ContainsKey(info))
                {
                    weaponPrefabs.Add(info, weapon);
                    weapon.SetActive(false);
                }
            }
        }
    }

    private void AssignAnimationIDs()
    {
        _animIDEquipWeapon = Animator.StringToHash("EquipWeapon");
        _animIDEquipMelee = Animator.StringToHash("EquipMelee");
        _animIDEquipRanged = Animator.StringToHash("EquipRanged");
        _animIDEquipItem = Animator.StringToHash("EquipItem");
        _animIDUnEquip = Animator.StringToHash("UnEquip");
    }

    public void EquipNew(ItemInfo data)
    {
        if (!weaponPrefabs.TryGetValue(data, out GameObject equip))
        {
            Debug.Log("해당 장착 아이템을 찾을 수 없습니다.");
            return;
        }

        if (!equip.TryGetComponent(out PlayerWeapon weaponData))
        {
            Debug.Log("무기 장착 아이템을 찾을 수 없습니다.");
            return;
        }
        
        var (weaponInfo, weaponType) = weaponData.GetWeaponData();
        var (consumableInfo, consumableType) = weaponData.GetConsumableData();
        
        if (weaponInfo == null && consumableInfo == null)
        {
            Debug.Log("해당 아이템 정보가 일치하지 않습니다.");
            return;
        }

        if (curWeaponPrefab != null)
        {
            if (curWeaponPrefab == equip)
            {
                UnEquip();
                return;
            }

            UnEquip(true);
        }

        if (weaponInfo != null)
        {
            switch (weaponType)
            {
                case PlayerWeaponType.Melee:
                    _equipMelee = true;
                    break;
                case PlayerWeaponType.Ranged:
                    _equipRanged = true;
                    break;
            }

            curWeapon = weaponInfo;
            curWeaponType = weaponType;
        }
        else
        {
            _equipItem = true;
            
            curWeaponType = consumableType;
        }
        
        curWeaponPrefab = equip;
        curWeaponData = weaponData;
        curWeaponPrefab.SetActive(true);

        UpdateAnimationBools();
    }
    
    public void UnEquip(bool isSwap = false)
    {
        GameObject weaponToUnEquip = curWeaponPrefab;
        
        curEquip = null;
        curWeaponType = PlayerWeaponType.None;
        _equipMelee = false;
        _equipRanged = false;
        _equipItem = false;

        if (isSwap) _animator.SetTrigger(_animIDUnEquip);
        UpdateAnimationBools();

        StartCoroutine(UnEquipCoroutine(weaponToUnEquip));
    }

    private IEnumerator UnEquipCoroutine(GameObject weaponToUnEquip)
    {
        yield return new WaitForSeconds(2f);
        
        if (weaponToUnEquip != null)
        {
            weaponToUnEquip.SetActive(false);
        }
    }

    private void UpdateAnimationBools()
    {
        _animator.SetBool(_animIDEquipMelee, _equipMelee);
        _animator.SetBool(_animIDEquipRanged, _equipRanged);
        _animator.SetBool(_animIDEquipItem, _equipItem);
        _animator.SetBool(_animIDEquipWeapon, _equipMelee || _equipRanged || _equipItem);
    }

    public void ToggleWeaponLocation()
    {
        _isWeaponOnHand = !_isWeaponOnHand; 
        
        curWeaponPrefab.transform.SetParent(_isWeaponOnHand ? weaponPivot : equipPivot, false);

        (Vector3 position, Vector3 rotation) = curWeaponData.GetEquipPosition(_isWeaponOnHand);
        curWeaponPrefab.transform.localPosition = position;
        curWeaponPrefab.transform.localRotation = Quaternion.Euler(rotation);
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
            damagable.TakeDamage(curEquip != null ? curWeapon.power : 10f);
        }
    }
}
