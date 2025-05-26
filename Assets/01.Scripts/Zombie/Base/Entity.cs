using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Entity : MonoBehaviour
{
    [SerializeField] protected Rigidbody _rigidbody;
    protected EntityStateMachine _stateMachine;
    public BaseStatus baseStatus;

    protected void Reset()
    {
        _rigidbody = GetComponent<Rigidbody>();
        if (_rigidbody == null)
        {
            _rigidbody = gameObject.AddComponent<Rigidbody>();
            _rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }
    }

    protected void Update()
    {
        _stateMachine.Update();
    }
}
