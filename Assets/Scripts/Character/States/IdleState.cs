using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : MonoBehaviour, IState
{
    [Header("Character Physics Data")]
    [SerializeField] CharacterPhysicsData data;

    [Header("Idle State")]
    [SerializeField] private Rigidbody2D charRb;
    [SerializeField] private Animator charAnimator;
    [SerializeField] GroundCheck groundCheck;

    private StateMachine machine;
    private States type = States.Idle;

    public StateMachine Machine
    {
        get { return machine; }
        set { machine = value; }
    }

    public States Type => type;

    public void Enter()
    {
        charAnimator.SetFloat("movX", 0);
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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (CoyoteTimeExpired())
            {
                return; // Evita que salte si ya termino el coyote time
            }
            if (CanJump())
            {
                // Evita que se acumule la velocidad en Y
                charRb.velocity = new Vector2(charRb.velocity.x, 0);

                charRb.AddForce(new Vector2(0, data.jumpForce), ForceMode2D.Impulse);
            }
        }

        if (!groundCheck.grounded)
        {
            machine.ChangeState(States.Airborne);
        }
    }

    private bool CoyoteTimeExpired() // True si no esta en el suelo y el tiempo en el aire es mayor al coyote time
    {
        return !groundCheck.grounded && machine.Character.AirTime >= machine.Character.Data.coyoteTime;
    }
    private bool CoyoteTime() // True esta dentro del coyote time
    {
        return machine.Character.AirTime < machine.Character.Data.coyoteTime;
    }
    private bool CanJump() // True esta en el suelo o dentro del coyote time
    {
        return (groundCheck.grounded || CoyoteTime());
    }
}
