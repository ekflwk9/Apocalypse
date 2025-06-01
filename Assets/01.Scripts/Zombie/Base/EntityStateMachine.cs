using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;



public class StateStruct
{
    Entity entity;
    protected Dictionary<EntityEnum, EntityState> StateDictionary = new Dictionary<EntityEnum, EntityState>();
    public EntityState CurrentState { get; protected set; }

    EntityStateMachine stateMachine;

    public virtual void Init(EntityStateMachine _StateMachine)
    {
        stateMachine = _StateMachine;
        foreach (KeyValuePair<EntityEnum, EntityState> state in StateDictionary)
        {
            state.Value.SetOwner(entity, _StateMachine);
        }
    }

    public void SetOwner(Entity _Entity)
    {
        entity = _Entity;
    }

    public void SetState(EntityState _State)
    {
        CurrentState = _State;
    }

    public virtual void SetState(EntityEnum _State)
    {
        CurrentState  = StateDictionary[_State];
    }

    public bool IsExist(EntityEnum _State)
    {
        return StateDictionary.ContainsKey(_State);
    }
}

public class EntityStateMachine
{
    StateStruct allState;

    public EntityEnum GetState() => allState.CurrentState._EntityEnum;

    public void SetState(EntityEnum _State)
    {
        if(false == allState.IsExist(_State))
        {
#if UNITY_EDITOR
            Debug.LogError($"State {_State} does not exist in the state machine.");
#endif
            return;
        }

        if (allState.CurrentState != null)
        {
            allState.CurrentState.Exit();
        }
        allState.SetState(_State);
        allState.CurrentState.Enter();
    }

    public void SetState(EntityState _State)
    {
        if (allState.CurrentState != null)
        {
            allState.CurrentState.Exit();
        }
        allState.SetState(_State);
        if (allState.CurrentState != null)
        {
            allState.CurrentState.Enter();
        }
    }

    public virtual void Init(StateStruct _StateStruct, Entity _Owner)
    {
        _StateStruct.SetOwner(_Owner);
        _StateStruct.Init(this);
        allState = _StateStruct;
    }

    public void Update()
    {
        if (allState.CurrentState != null)
        {
            allState.CurrentState.Update();
        }
    }
}
