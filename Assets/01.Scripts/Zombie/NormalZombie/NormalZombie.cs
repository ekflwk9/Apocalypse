using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalZombie : Entity
{
    private void Awake()
    {
        _stateMachine = new EntityStateMachine();
        _stateMachine.Init(new NormalZombieStateStruct(), this);

        baseStatus = new BaseStatus();

        _stateMachine.SetState(EntityEnum.Idle);
    }
}
