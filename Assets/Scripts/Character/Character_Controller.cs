using Unity.VisualScripting;
using UnityEngine;

public class Character_Controller : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    [SerializeField] private CharacterPhysicsData data;

    [SerializeField] private StateMachine stateMachine;

    [SerializeField] public float maxHealth = 100;
    [SerializeField] public float currentHealth = 100;
    [SerializeField] float airTime;
    //[SerializeField] private float resetBool;
    private Iinteractable inRangeIntaraction;

    public float initialFallPosition = 0;
    public bool isAlive = true;
    public bool jumping = false;

    public int coin =0;

    // Events
    public delegate void OnLifeChangeHandler(float life);
    public event OnLifeChangeHandler OnLifeChange;


    public float AirTime => airTime;
    public CharacterPhysicsData Data => data;

    private void Update()
    {
        //FallDistance();
    }

    public void ResetAirTime()
    {
        airTime = 0;
    }

    public void UpdateStateMachine()
    {
        stateMachine.UpdateStateMachine();
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

    //public void FallDistance() // Este es llamado por el Player controller todo el tiempo
    //{
    //    if (IsFalling() && !isFalling) // Esta chequea la velocidad del rigidbody en Y y el air time
    //    {
    //        isFalling = true;
            
    //        initialFallPosition = rb.position.y;
    //        Debug.Log("Is falling");
    //    }
    //    if (Grounded() && isFalling)
    //    {
    //        isFalling = false;
    //        FallDamage();
    //    }
    //}

    //public void FallDamage()
    //{
    //    float totalFallDistance = initialFallPosition - rb.position.y;

    //    if (totalFallDistance > data.fallDamageDistance)
    //    {
    //        GetDamage(Mathf.Round(data.fallDamage + data.fallDamage * ( data.fallDamageMultiplier * totalFallDistance)));
    //    }
    //}


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
}
