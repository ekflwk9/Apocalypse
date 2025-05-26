using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.VersionControl.Asset;

public class NormalZombieStateStruct : StateStruct
{
    public override void Init()
    {
        IdleState idleState = new IdleState();
        RunState runState = new RunState();
        StateDictionary.Add(EntityEnum.Idle, idleState);
        StateDictionary.Add(EntityEnum.Run, runState);
        base.Init();
    }
}

public class IdleState : EntityState
{
    public override void Enter()
    {
    }
    public override void Update()
    {
        // Logic for idle state
    }
    public override void Exit()
    {
    }
}

public class RunState : EntityState
{
    Rigidbody entityRigidbody;
    public override void Enter()
    {
        entityRigidbody = entity.GetComponent<Rigidbody>();
    }
    public override void Update()
    {
        Vector3 currentVelocity = entityRigidbody.velocity;
        entityRigidbody.velocity = new Vector3(0, currentVelocity.y, 5);
    }
    public override void Exit()
    {
    }
}
