using System;
using GameItem;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerEquip : MonoBehaviour
{
    public GameObject[] Weapons;
    public WeaponInfo curEquip;
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

    // public void EquipNew(WeaponInfo weapon)
    // {
    //      //curEqip = weapon;
    //      받아온 무기정보의 종류와 같은 무기 활성화
    // }

    //테스트 코드
    public void EquipNew(int index)
    {
        if(index >= Weapons.Length) return;
        if(SelectWeapon != null)
        {
            if (SelectWeapon.transform.parent != equipPivot)
            {
                return;
            }
            
            if (SelectWeapon == Weapons[index])
            {
                Unequip();
                return;
            }
            
            Unequip();
        }
        
        SelectWeapon = Weapons[index];
        _equipMelee = true;
        SelectWeapon.SetActive(true);
        _animator.SetBool(_animIDEquipMelee, true);
    }
    //
    
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
        SelectWeapon.transform.SetParent(_isWeaponOnHand ? weaponPivot : equipPivot, false);
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
