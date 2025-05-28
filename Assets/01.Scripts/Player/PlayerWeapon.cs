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

    public (WeaponInfo, PlayerWeaponType) GetWeaponData()
    {
        return (_weponData, type);
    }
}