using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimCallBack : MonoBehaviour
{
    [SerializeField] private Entity _entity;

    private void Reset()
    {
        _entity = GetComponentInParent<Entity>();
    }

    void Attack_1()
    {
        _entity.Attack_1();
    }

    void Attack_2()
    {
        _entity.Attack_2();
    }

}
