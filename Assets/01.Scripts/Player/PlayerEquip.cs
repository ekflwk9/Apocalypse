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
    
    [SerializeField] private Animator _animator;
    private int _animIDEquip;
    private int _animIDEquipMelee;
    private int _animIDEquipRanged;
    private int _animIDEquipItem;
    private int _animIDUnEquip;
    
    private bool _equipMelee;
    public bool EquipMelee => _equipMelee;
    private bool _equipRanged;
    private bool _equipItem;
    private bool _isWeaponOnHand = false;

    private void Start()
    {
        AssignAnimationIDs();
        
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
        _animIDEquip = Animator.StringToHash("Equip");
        _animIDEquipMelee = Animator.StringToHash("EquipMelee");
        _animIDEquipRanged = Animator.StringToHash("EquipRanged");
        _animIDEquipItem = Animator.StringToHash("EquipItem");
        _animIDUnEquip = Animator.StringToHash("UnEquip");
    }
    
    public void EquipItem(ItemInfo itemInfo)
    {
        //if(_unEquipCoroutine != null || _equipCoroutine != null) return;
        if (curWeaponPrefab == null)
        {
            EquipNew(itemInfo); // 아무 것도 장착되지 않았을 때
            return;
        }

        if (itemInfo == curWeaponData.GetItemData())
        {
            UnEquip(); // 같은 아이템이면 해제
            return;
        }

        Swap(itemInfo); // 다른 아이템이면 스왑
    }
    
    private Coroutine _equipCoroutine;
    private Coroutine _unEquipCoroutine;
    
    private void EquipNew(ItemInfo data)
    {
        if (!weaponPrefabs.TryGetValue(data, out GameObject equip))
        {
#if UNITY_EDITOR

            Debug.Log("해당 장착 아이템을 찾을 수 없습니다.");
#endif
            return;
        }

        if (!equip.TryGetComponent(out PlayerWeapon equipData))
        {
#if UNITY_EDITOR

            Debug.Log("무기 장착 아이템을 찾을 수 없습니다.");
#endif
            return;
        }
        
        var (weaponInfo, weaponType) = equipData.GetWeaponData();
        var (consumableInfo, consumableType) = equipData.GetConsumableData();
        
        if (weaponInfo == null && consumableInfo == null)
        {
#if UNITY_EDITOR

            Debug.Log("해당 아이템 정보가 일치하지 않습니다.");
#endif
            return;
        }

        curWeapon = weaponInfo != null ? weaponInfo : null;
        curWeaponType = weaponInfo != null ? weaponType : consumableType;
        curWeaponPrefab = equip;
        curWeaponData = equipData;
        curEquip = data;
        
        _equipMelee = weaponType == PlayerWeaponType.Melee;
        _equipRanged = weaponType == PlayerWeaponType.Ranged;
        _equipItem = weaponInfo == null;
        
        
        curWeaponPrefab.SetActive(true);

        UpdateAnimationBools();
        
        _equipCoroutine = StartCoroutine(WaitForEquip(curWeaponPrefab, curWeaponData));
    }

    private IEnumerator WaitForEquip(GameObject obj, PlayerWeapon data)
    {
        _animator.SetBool(_animIDEquip, _equipMelee || _equipRanged || _equipItem);
        yield return WaitUntilAnimationEnd("Equip");
        ToggleWeaponLocation(obj, data);
        _equipCoroutine = null;
    }
    
    public void UnEquip()
    {
        if(curWeaponPrefab == null) return;
        
        if (curWeaponPrefab.activeInHierarchy)
        {
            GameObject weaponToUnEquip = curWeaponPrefab;
            PlayerWeapon weaponDataToUnEquip = curWeaponData;
            _animator.SetTrigger(_animIDUnEquip);
            _unEquipCoroutine = StartCoroutine(WaitForUnEquip(weaponToUnEquip, weaponDataToUnEquip)); 
        }
        
        curWeaponPrefab = null;
        curWeaponData = null;
        curWeapon = null;
        curWeaponType = PlayerWeaponType.None;
        
        _equipMelee = false;
        _equipRanged = false;
        _equipItem = false;
        
        UpdateAnimationBools();
    }

    private IEnumerator WaitForUnEquip(GameObject obj, PlayerWeapon data)
    {
        UpdateAnimationBools();
        yield return WaitUntilAnimationEnd("UnEquip");
        ToggleWeaponLocation(obj, data);
        if (obj != null)
        {
            obj.SetActive(false);
        }
        _unEquipCoroutine = null;
    }

    private void Swap(ItemInfo data)
    {
        UnEquip();
        EquipNew(data);
    }

    private void UpdateAnimationBools()
    {
        _animator.SetBool(_animIDEquipMelee, _equipMelee);
        _animator.SetBool(_animIDEquipRanged, _equipRanged);
        _animator.SetBool(_animIDEquipItem, _equipItem);
    }

    public void ToggleWeaponLocation(GameObject obj, PlayerWeapon data)
    {
        _isWeaponOnHand = !_isWeaponOnHand; 
        
        obj.transform.SetParent(_isWeaponOnHand ? weaponPivot : equipPivot, false);
        
        (Vector3 position, Vector3 rotation) = data.GetEquipPosition(_isWeaponOnHand);
        obj.transform.localPosition = position;
        obj.transform.localRotation = Quaternion.Euler(rotation);
    }
    
    private IEnumerator WaitUntilAnimationEnd(string stateName)
    {
        int upperBodyLayer = _animator.GetLayerIndex("UpperBody");
        // 현재 재생 중인 상태로 바뀔 때까지 대기
        while (!_animator.GetCurrentAnimatorStateInfo(upperBodyLayer).IsName(stateName))
            yield return null;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Monster")) return;
        IDamagable damagable;
        bool isEnemy = other.TryGetComponent(out damagable);
        if (!isEnemy)
        {
            return;
        }
        else
        {
            damagable.TakeDamage(curWeapon != null ? curWeapon.power : 10f);
        }
    }
}
