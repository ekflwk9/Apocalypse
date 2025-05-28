using System.Collections;
using System.Collections.Generic;
using GameItem;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] private WeaponInfo _weponData;

    public WeaponInfo GetWeaponData()
    {
        return _weponData;
    }
}
