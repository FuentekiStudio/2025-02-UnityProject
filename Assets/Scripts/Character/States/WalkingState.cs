using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingState : MonoBehaviour, IState
{
    [Header("Character Physics Data")]
    [SerializeField] CharacterPhysicsData data;

    [Header("Walking State")]
    [SerializeField] private Rigidbody2D charRb;
    [SerializeField] private Animator charAnimator;
    [SerializeField] private SpriteRenderer charSpriteRenderer;
    [SerializeField] GroundCheck groundCheck;

    private StateMachine machine;
    private States type = States.Walking;

    public StateMachine Machine
    {
        get { return machine; }
        set { machine = value; }
    }
    public States Type => type;

    public void Enter()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        charRb.AddForce(new Vector2(horizontal * data.speedKickoff, 0), ForceMode2D.Impulse);
    }

    public void Exit()
    {
        
    }

    public void UpdateState()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");

        if (horizontal < 0)
        {
            if (charRb.velocity.x > -data.maxSpeed)
            {
                charRb.AddForce(new Vector2(-1 * data.speed * Time.deltaTime, 0), ForceMode2D.Force);
            }

            if (!charSpriteRenderer.flipX)
            {
                charSpriteRenderer.flipX = true;
            }
        }
        else if (horizontal > 0)
        {
            if (charRb.velocity.x < data.maxSpeed)
            {
                charRb.AddForce(new Vector2(data.speed * Time.deltaTime, 0), ForceMode2D.Force);
            }

            if (charSpriteRenderer.flipX)
            {
                charSpriteRenderer.flipX = false;
            }
        }
        else
        {
            charRb.AddForce(-charRb.velocity, ForceMode2D.Impulse);
            machine.ChangeState(States.Idle);
            return;
        }

        charAnimator.SetFloat("movX", horizontal);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (CoyoteTimeExpired())
            {
                return;
            }
            if (CanJump())
            {
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
