using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirborneState : MonoBehaviour, IState
{
    [Header("Airborne State")]
    [SerializeField] private Rigidbody2D charRb;
    [SerializeField] private Animator charAnimator;
    [SerializeField] private SpriteRenderer charSpriteRenderer;
    [SerializeField] GroundCheck groundCheck;

    private StateMachine machine;
    private States type = States.Airborne;

    public StateMachine Machine
    {
        get { return machine; }
        set { machine = value; }
    }
    public States Type => type;

    public void Enter()
    {

    }

    public void Exit()
    {

    }

    public void UpdateState()
    {
        JumpAnimation();

        if (machine.Character.Horizontal < 0)
        {
            if (charRb.velocity.x > -machine.Character.Data.maxSpeed)
            {
                charRb.AddForce(new Vector2(-1 * machine.Character.Data.speed * Time.deltaTime, 0), ForceMode2D.Force);
            }

            if (!charSpriteRenderer.flipX)
            {
                charSpriteRenderer.flipX = true;
            }
        }
        else if (machine.Character.Horizontal > 0)
        {
            if (charRb.velocity.x < machine.Character.Data.maxSpeed)
            {
                charRb.AddForce(new Vector2(machine.Character.Data.speed * Time.deltaTime, 0), ForceMode2D.Force);
            }

            if (charSpriteRenderer.flipX)
            {
                charSpriteRenderer.flipX = false;
            }
        }

        if (groundCheck.grounded)
        {
            if (machine.Character.Horizontal != 0)
            {
                machine.ChangeState(States.Walking);
            }
            else
            {
                machine.ChangeState(States.Idle);
            }

            machine.Character.jumping = false;
        }
    }


    private void JumpAnimation()
    {
        if (groundCheck.grounded)
        {
            charAnimator.SetBool("isGrounded", true);
            charAnimator.SetFloat("movY", 0);
        }
        if (charRb.velocity.y > 1f)
        {
            charAnimator.SetFloat("movY", 1);
            charAnimator.SetBool("isGrounded", false);
        }
        else if (!groundCheck.grounded && charRb.velocity.y < 7f)
        {
            charAnimator.SetFloat("movY", -1);
            // Probar aceleracion de caida
            charRb.AddForce(machine.Character.Data.fuerzaExtra * Physics.gravity);
        }
    }
}
