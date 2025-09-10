using UnityEngine;

public class Arrow : MonoBehaviour
{
    private float damage = 50f; // Default damage amount
    private bool isFired = false; // Tracks if the arrow has been fired

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Only move the arrow forward after it is fired
        if (isFired)
        {
            // Keep moving the arrow forward
        }
    }

    // This method allows the TrapArrow to set the damage value
    public void SetDamage(float damageAmount)
    {
        damage = damageAmount;
        isFired = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the arrow hits the player
        Character_Controller player = collision.GetComponent<Character_Controller>();
        if (player && isFired)
        {
            // Apply damage to the player if the arrow has been fired
            player.GetDamage(damage);
            Debug.Log("Player hit by the arrow!");

            // Destroy the arrow after it hits the player
            Destroy(gameObject);
        }
    }
}
