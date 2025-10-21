using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    StateMachine Machine { get; set; }
    States Type { get; }

    void Enter();
    void UpdateState();
    void Exit();
}
