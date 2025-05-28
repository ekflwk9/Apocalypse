using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerWeaponType
{
    None,
    Melee,
    Ranged,
    RangedAuto
}
public class PlayerWeapon : MonoBehaviour
{
    [SerializeField] private WeaponInfo _weponData;
    [SerializeField] private PlayerWeaponType type;
    
    [SerializeField] private Vector3 equipPosition;
    [SerializeField] private Vector3 equipRotation;
    [SerializeField] private Vector3 handPosition;
    [SerializeField] private Vector3 handRotation;

    [SerializeField] private Transform bulletOrigin;
    [SerializeField] private Transform bulletDirectionPoint;
    [SerializeField] private float _range;

    public float Range
    {
        get => _range;
    }

    public (WeaponInfo, PlayerWeaponType) GetWeaponData()
    {
        return (_weponData, type);
    }

    public (Vector3, Vector3) GetEquipPosition(bool isOnHand)
    {
        Debug.Log($"GetthePoint! {isOnHand}");
        Vector3 position = isOnHand ? handPosition : equipPosition;
        Vector3 rotation = isOnHand ? handRotation : equipRotation;
        return (position, rotation);
    }

    public (Vector3, Vector3) GetBulletStartPoint()
    {
        Vector3 direction = (bulletDirectionPoint.position - bulletOrigin.position).normalized;
        return (bulletOrigin.position, direction);
    }
}
