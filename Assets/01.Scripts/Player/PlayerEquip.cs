using System;
using GameItem;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerEquip : MonoBehaviour
{
    public WeaponInfo curEquip;
    [SerializeField] private Transform equipPivot;
    [SerializeField] private Transform weaponPivot;
    public BoxCollider meleeCollider;
    
    [SerializeField] private Animator _animator;
    private int _animIDEquipMelee;
    private int _animIDEquipRanged;
    private bool _equipMelee;
    private bool _equipRanged;
    [SerializeField] private bool _toggleMelee = false;
    
    //테스트 코드
    public GameObject[] TestWeapons;
    public GameObject SelectWeapon;
    //
    
    private void Start()
    {
        AssignAnimationIDs();
        meleeCollider.enabled = false;
        foreach (var weapon in TestWeapons)
        {
            weapon.SetActive(false);
        }
    }

    private void AssignAnimationIDs()
    {
        _animIDEquipMelee = Animator.StringToHash("EquipMelee");
        _animIDEquipRanged = Animator.StringToHash("EquipRanged");
    }

    // public void EquipNew(WeaponInfo weapon)
    // {
    //      받아온 무기정보의 종류와 같은 무기 활성화
    // }

    //테스트 코드
    public void EquipNew(int index)
    {
        SelectWeapon = TestWeapons[index];
        _equipMelee = true;
        SelectWeapon.SetActive(true);
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
        SelectWeapon.transform.SetParent(weaponPivot, false);
    }

    public void MoveWeaponToBack()
    {
        //curEquip.GameObject().transform.SetParent(equipPivot, false);
        SelectWeapon.transform.SetParent(equipPivot, false);
    }
    
    public void ToggleMeleeCollider()
    {
        if (_equipMelee)
        {
            meleeCollider.enabled = !_toggleMelee;
            _toggleMelee = !_toggleMelee;
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
            damagable.TakeDamage(curEquip != null ? curEquip.power : 10f);
        }
    }
}
