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
        bool hasCollider = TestWeapon.TryGetComponent(out collider);
        if (hasCollider)
        {
            Debug.Log("콜라이더 찾음");
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

    public void MoveWeaponToHand()
    {
        //curEquip.GameObject().transform.SetParent(weaponPivot, false);
        TestWeapon.transform.SetParent(weaponPivot, false);
    }

    public void MoveWeaponToBack()
    {
        //curEquip.GameObject().transform.SetParent(equipPivot, false);
        TestWeapon.transform.SetParent(equipPivot, false);
    }
    
    public void EnableWeaponCollider()
    {
        if (collider != null)
        {
            Debug.Log("콜라이더 켰다");
            collider.enabled = true;
        }
    }

    public void DisableWeaponCollider()
    {
        if (collider != null)
        {
            Debug.Log("콜라이더 껐다");
            collider.enabled = false;
        }
    }
}
