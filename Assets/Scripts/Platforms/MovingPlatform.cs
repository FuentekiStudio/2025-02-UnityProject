using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{

    [SerializeField] float raycastRange = 1.0f;
    [SerializeField] float velocity = 100.0f;
    [SerializeField] LayerMask anchor;

    RaycastHit2D raycastLeft;
    RaycastHit2D raycastRight;

    private BoxCollider2D boxCollider;
    private Rigidbody2D rb;

    private Vector2 movementDirection = Vector2.right;


    // Start is called before the first frame update
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {

        Vector2 colliderBottomCenter = (Vector2)transform.position + boxCollider.offset; //
        Vector2 raycastOriginLeft = colliderBottomCenter + new Vector2(-boxCollider.size.x /3, 0);
        Vector2 raycastOriginRight = colliderBottomCenter + new Vector2(boxCollider.size.x /3, 0);

        raycastLeft = Physics2D.Raycast(raycastOriginLeft, Vector2.left, raycastRange, anchor);
        raycastRight = Physics2D.Raycast(raycastOriginRight, Vector2.right, raycastRange, anchor);

        Debug.DrawRay(raycastOriginLeft, Vector2.left * raycastRange, Color.blue);
        Debug.DrawRay(raycastOriginRight, Vector2.right * raycastRange, Color.blue);




        if (RightHit())
        {
            movementDirection = Vector2.left;
        }
        if (LeftHit())
        {
            movementDirection = Vector2.right;
        }

        rb.velocity = movementDirection * velocity * Time.fixedDeltaTime;
    }

    private void FixedUpdate()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Rigidbody2D playerRb = collision.GetComponent<Rigidbody2D>();
            Character_Controller control = collision.GetComponent<Character_Controller>();
            if (LeftHit())
            {
                if (playerRb.velocity.x <0)
                {
                    playerRb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0.5f, 0.01f * Time.deltaTime), rb.velocity.y);                   
                }               
            }

            if (RightHit())
            {
                if (playerRb.velocity.x >0)
                {
                    playerRb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, -0.5f, 0.01f * Time.deltaTime), rb.velocity.y);                    
                }                
            }
            //if (playerRb.velocity.x <= rb.velocity.x && playerRb.velocity.x >= -rb.velocity.x)
            //{
            //    control.isRunning = false;
            //}
            
        }
    }
    private bool LeftHit()
    {
        return raycastLeft.collider != null;
    }

    private bool RightHit()
    {
        return raycastRight.collider != null;
    }
}
