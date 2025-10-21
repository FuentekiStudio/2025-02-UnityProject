using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingState : MonoBehaviour, IState
{
    private StateMachine machine;
    private States type = States.Idle;

    public StateMachine Machine => machine;
    public States Type => type;

    public void Enter()
    {
        
    }

    public void Exit()
    {
        
    }

    public void UpdateState()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");

        switch (horizontal)
        {
            case -1f:
                machine.Character.MoveLeft();
                break;

            case 1:
                machine.Character.MoveRight();
                break;

            case 0:
                machine.ChangeState(States.Idle);
                break;
        }
    }
}
