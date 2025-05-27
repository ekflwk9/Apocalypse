using System;
using GameItem;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerEquip : MonoBehaviour
{
    public WeaponInfo curEquip;
    public Collider collider;
    [SerializeField] private Transform equipPivot;
    [SerializeField] private Transform weaponPivot;
    
    [SerializeField] private Animator _animator;
    private int _animIDEquipMelee;
    private int _animIDEquipRanged;
    
    //테스트 코드
    public GameObject TestWeapon;
    //
    
    private void Start()
    {
        AssignAnimationIDs();
    }

    private void AssignAnimationIDs()
    {
        _animIDEquipMelee = Animator.StringToHash("EquipMelee");
        _animIDEquipRanged = Animator.StringToHash("EquipRanged");
    }

    // public void EquipNew(WeaponInfo weapon)
    // {
    //     Unequip();
    //     //curEquip = Instantiate(weapon.장착프리팹, equipPivot).GetComponent<Equip>();
    //     //collider = curEquip.GetComponent<Collider>();
    //     //collider.enabled = false;
    // }

    //테스트 코드
    public void EquipNew(GameObject weapon)
    {
        TestWeapon = weapon;
        TestWeapon.transform.SetParent(equipPivot, false);
        bool hasCollider = TestWeapon.TryGetComponent<Collider>(out collider);
        if (hasCollider)
        {
            collider.enabled = false;
        }
        _animator.SetBool(_animIDEquipMelee, true);
    }
    //
    
    public void Unequip()
    {
        if (curEquip != null)
        {
            //파괴혹은 풀에 반환
            curEquip = null;
        }
    }

    public void Aim()
    {
        TestWeapon.transform.SetParent(weaponPivot, false);
    }
    
    public void EnableWeaponCollider()
    {
        if (curEquip != null)
        {
            collider.enabled = true;
        }
    }

    public void DisableWeaponCollider()
    {
        if (curEquip != null)
        {
            collider.enabled = false;
        }
    }
}
