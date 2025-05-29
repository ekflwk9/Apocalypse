using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StringHelper
{
    public const string Empty = "asdf";
}

public static class TagHelper
{
    public const string Player = "Player";
    public const string Monster = "Monster";
    public const string Item = "Item";
    public const string Weapon = "Weapon";
    public const string Zombie = "Zombie";
}

public static class LayerHelper
{
    public const string Player = "Player";
    public const string Monster = "Monster";
    public const string Item = "Item";
    public const string Weapon = "Weapon";
    public const string Zombie = "Zombie";
    public static int GetLayer(string layerName)
    {
        return LayerMask.NameToLayer(layerName);
    }
}
