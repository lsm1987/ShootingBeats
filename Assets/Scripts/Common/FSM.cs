using UnityEngine;
using System;
using System.Collections.Generic;

// FSM 상태
// Enum 을 generic 인자로 쓰기: http://stackoverflow.com/questions/79126/create-generic-method-constraining-t-to-an-enum
public abstract class FSMState<TStateType>
    where TStateType : struct, IConvertible
{
    public abstract TStateType _StateType { get; }

    /// <summary>
    /// 상태 진입 후에 호출
    /// </summary>
    public virtual void OnEnter(TStateType prevState)
    {
    }

    /// <summary>
    /// 상태 이탈 전에 호출
    /// </summary>
    public virtual void OnLeave(TStateType nextState)
    {
    }

    /// <summary>
    /// 상태 갱신
    /// </summary>
    public virtual void OnUpdate()
    {
    }
}

// FSM
public abstract class FSM<TStateType, TState>
    where TStateType : struct, IConvertible
    where TState : FSMState<TStateType>
{

    private Dictionary<TStateType, TState> _states = new Dictionary<TStateType, TState>();
    public TState _CurrentState { get; private set; }
    protected abstract TStateType _InvalidStateType { get; } // 무효한 상태. 최초 상태 진입 시에는 이전 상태가 없으므로 이를 표시하기 위함

    public void AddState(TState state)
    {
        _states.Add(state._StateType, state);
    }

    public void SetState(TStateType stateType)
    {
        TState nextState = null;
        _states.TryGetValue(stateType, out nextState);
        if (nextState == null)
        {
            Debug.LogError("[FSM] Invalid StateType:" + stateType.ToString());
            return;
        }

        // 현재 상태에서 빠져나감
        if (_CurrentState != null)
        {
            _CurrentState.OnLeave(stateType);
        }
        TState prevState = _CurrentState;

        // 현재 상태 새로 지정
        _CurrentState = nextState;
        _CurrentState.OnEnter((prevState != null) ? prevState._StateType : _InvalidStateType);
    }
}
