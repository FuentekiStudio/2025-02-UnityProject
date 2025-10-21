using Unity.VisualScripting;
using UnityEngine;

public class Character_Controller : MonoBehaviour
{
    private Character_SFX_component characterSFX;
    public SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private Animator animator;

    [SerializeField] private StateMachine stateMachine;

    [SerializeField] float speed = 50.0f;
    [SerializeField] float maxSpeed = 50.0f;

    [SerializeField] float jumpForce = 7.0f;
    [SerializeField] float jumpColdTime = 0.25f;
    [SerializeField] float fuerzaExtra = 1.1f;

    [SerializeField] public float maxHealth = 100;
    [SerializeField] public float currentHealth = 100;
    [SerializeField] float airTime;

    public void ResetAirTime()
    {
        airTime = 0;
    }

    [SerializeField] float coyoteTime = 0.25f;
    [SerializeField] private float resetBool;

    [SerializeField] private float fallDamage= 1f;
    [SerializeField] private float fallDamageMultiplier = 0.5f;
    [SerializeField] private float fallDamageDistance = 4f;
 

    public float initialFallPosition = 0;

    
    private bool isFalling = false;


    [SerializeField] LayerMask suelo;

    public int coin =0;

    // Events
    public delegate void OnLifeChangeHandler(float life);
    public event OnLifeChangeHandler OnLifeChange;


    private Iinteractable inRangeIntaraction;


    private GroundCheck groundCheck;

    public bool isAlive = true;
    public bool jumping = false;


    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        groundCheck = GetComponentInChildren<GroundCheck>();
        characterSFX = GetComponent<Character_SFX_component>();
    }

    // Update is called once per frame
    private void Update()
    {
        FallDistance();
        if (Grounded())
        {
            airTime = 0;
            
            if (jumping)
            {
                resetBool += Time.deltaTime;

                if (resetBool > 0.3)
                {
                    jumping = false;
                    resetBool = 0;
                }
            }
        }
        else
        {
            airTime += Time.deltaTime;
            if (airTime > jumpColdTime) {
                jumping = false;
            }
        }
        
    }

    private void FixedUpdate()
    {
        StopMovement();
        JumpAnimation();
    }

    public void UpdateStateMachine()
    {
        stateMachine.UpdateStateMachine();
    }

    public void MoveRight()
    {
        if (rb.velocity.x < maxSpeed)
        {
            rb.AddForce(new Vector2(speed * Time.deltaTime, 0), ForceMode2D.Force);
        }

        if (spriteRenderer.flipX)
        {
            spriteRenderer.flipX = false;
        }

        animator.SetFloat("movX", 1);
    }

    public void MoveLeft()
    {   
        if (rb.velocity.x > -maxSpeed)
        {
            rb.AddForce(new Vector2(-1 * speed * Time.deltaTime, 0), ForceMode2D.Force);
        }

        if (!spriteRenderer.flipX)
        {
            spriteRenderer.flipX = true;
        }

        animator.SetFloat("movX", -1);
    }

    public void StopMovement()
    {
        if (rb.velocity.x > maxSpeed * -0.8f && rb.velocity.x < maxSpeed * 0.8f)
        {
            animator.SetFloat("movX", 0);
        }       
    }

    public void Jump()
    {   
        if (CoyoteTimeExpired())
        {
            return; // Evita que salte si ya termino el coyote time
        }
        if (CanJump())
        {
            // Evita que se acumule la velocidad en Y
            rb.velocity = new Vector2(rb.velocity.x, 0);
            
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            //Debug.Log(rb.velocity.y);

            if (rb.velocity.y > 1f)
                jumping = true;
        }  
    }
    private void JumpAnimation()
    {
        if (Grounded())
        {
            animator.SetBool("isGrounded", true);
            animator.SetFloat("movY", 0);
        }
        if (rb.velocity.y > 1f)
        {
            animator.SetFloat("movY", 1);
            animator.SetBool("isGrounded", false);
        }
        else if (!Grounded() && rb.velocity.y < 7f)
        {
            animator.SetFloat("movY", -1);
            // Probar aceleracion de caida
            rb.AddForce(fuerzaExtra * Physics.gravity);
        }
    }

    public void GetDamage(float damage)
    {
        currentHealth -= damage;


        if (currentHealth <=0)
        {
            currentHealth = 0;
            isAlive = false;
            // Temporary
            GameManager.instanceGM.ResetTimeVariables();
            GameManager.instanceGM.ResetCollectableVariables();
            Scene_Manager.ReloadScene(); // TO BE CHANGED - this should be removed and called once we run the "death animation" for the character
        }

        OnLifeChange?.Invoke(currentHealth);
    }

    public void Healing(float heal)
    {
        if (currentHealth + heal > maxHealth)
        {
            currentHealth = maxHealth;
        }
        else
        {
            currentHealth += heal;
        }

        OnLifeChange?.Invoke(currentHealth);
    }

    public void AddCoin()
    {
        PlayerController.instance.AddCoin();
    }

    public void FallDistance() // Este es llamado por el Player controller todo el tiempo
    {
        if (IsFalling() && !isFalling) // Esta chequea la velocidad del rigidbody en Y y el air time
        {
            isFalling = true;
            
            initialFallPosition = rb.position.y;
            Debug.Log("Is falling");
        }
        if (Grounded() && isFalling)
        {
            isFalling = false;
            FallDamage();
        }
    }

    public void FallDamage()
    {
        float totalFallDistance = initialFallPosition - rb.position.y;

        if (totalFallDistance > fallDamageDistance)
        {
            GetDamage(Mathf.Round(fallDamage + fallDamage * ( fallDamageMultiplier * totalFallDistance)));
        }
    }

    public bool IsFalling()
    {
        return (!Grounded() && rb.velocity.y < 0);
        
    }


    void OnTriggerStay2D(Collider2D other)
    {
        // Check if the object has the "Interactable" interface
        Iinteractable interactable = other.GetComponent<Iinteractable>();
        if (interactable!=null)
        {
            //Debug.Log("load");
            inRangeIntaraction = interactable;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        Iinteractable interactable = other.GetComponent<Iinteractable>();
        if (interactable != null)
        {
            //Debug.Log("unload");
            inRangeIntaraction = null; // Clear reference when leaving range
        }
    }

    public void InteractWithObject()
    {
        if (inRangeIntaraction!=null)
        {
           // Debug.Log("Interacting with: " + inRangeIntaraction);
            inRangeIntaraction.ExecuteInteraction();
        }
    }

    private bool Grounded() // True si alguno de los raycast hace contacto
    {
        return groundCheck.grounded;
    }
    private bool CoyoteTimeExpired() // True si no esta en el suelo y el tiempo en el aire es mayor al coyote time
    {
        return !Grounded() && airTime >= coyoteTime;
    }
    private bool CoyoteTime() // True esta dentro del coyote time
    {
        return airTime < coyoteTime;
    }
    private bool CanJump() // True esta en el suelo o dentro del coyote time
    {
        return (Grounded() || CoyoteTime()) && !jumping ;
    }
}
