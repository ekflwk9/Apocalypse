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
    public const string Obstacle = "Obstacle";
}

public static class LayerHelper
{
    public const string Player = "Player";
    public const string Monster = "Monster";
    public const string Item = "Item";
    public const string Weapon = "Weapon";
    public const string Zombie = "Zombie";
    public const string Obstacle = "Obstacle";

    // 초기화 할 때 사용하는거
    public static int InitLayer(string layerName)
    {
        return LayerMask.NameToLayer(layerName);
    }


    // overlap에서 layer값 사용할 때
    public static int GetLayer(string layerName)
    {
        return 1 << LayerMask.NameToLayer(layerName);
    }
}
