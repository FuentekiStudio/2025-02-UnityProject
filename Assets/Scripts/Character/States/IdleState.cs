using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : MonoBehaviour, IState
{
    private StateMachine machine;
    private States type = States.Idle;

    public StateMachine Machine => machine;
    public States Type => type;

    public void Enter()
    {
        machine.Character.StopMovement();
    }

    public void Exit()
    {
        
    }

    public void UpdateState()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");

        if (horizontal != 0)
        {
            Machine.ChangeState(States.Walking);
        }
    }
}
