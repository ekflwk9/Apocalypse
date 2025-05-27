using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStatus
{
    public float RunSpeed { get; protected set; } = 5f;
    public float WalkSpeed { get; protected set; } = 2f;
    public float Damage { get; protected set; } = 10f;

    public float CrowlSpeed { get; protected set; } = 0.5f;

    public bool IsHalf()
    {
        return currentHp <= maxHp * 0.5f;
    }

    private float currentHp;

    public float CurrentHp
    {
        get
        {
            return currentHp;
        }
        set
        {
            currentHp = Mathf.Clamp(value, 0, maxHp);
            if (currentHp <= 0)
            {
                currentHp = 0;
            }
        }
    }

    private float maxHp;

    public float MaxHp
    {
        get => maxHp;
        set
        {
            maxHp = Mathf.Max(value, 0);
            if (currentHp > maxHp)
            {
                currentHp = maxHp;
            }
        }
    }
}
