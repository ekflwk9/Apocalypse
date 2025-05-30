using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseStatus
{
    public BaseStatus()
    {
        currentHp = maxHp;
    }

    public void Init()
    {
        currentHp = maxHp;
    }

    public void SetStatus(float _DetectedRange, float _AttackRange, float _DetectedAngle = 45)
    {
        DetectedRange = _DetectedRange;
        AttackRange = _AttackRange;
        DetectedAngle = _DetectedAngle;
    }

    public float RunSpeed { get; protected set; } = 5f;
    public float WalkSpeed { get; protected set; } = 1f;
    public float Damage { get; protected set; } = 10f;

    public float DetectedRange { get; protected set; } = 10f;

    public float DetectedAngle { get; protected set; } = 45f;

    public float AttackRange { get; protected set; } = 2f;

    public float CrowlSpeed { get; protected set; } = 0.5f;

    public Vector3 DetectedLocation { get; set; }

    public bool IsHalf()
    {
        return currentHp <= maxHp * 0.5f;
    }

    public bool IsQuater()
    {
        if(currentHp <= maxHp * 0.25f)
        {
           int value = Random.Range(0, 4);
            if(value == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
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

    private float maxHp = 100;

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
