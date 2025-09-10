using UnityEngine;

public class TrapPikes : MonoBehaviour
{
    public Character_Controller player;

    [HideInInspector] public Animator m_Animator;
    private BoxCollider2D m_BoxCollider;
    private Rigidbody2D m_Rigidbody;

    [SerializeField] private float damage;
    [SerializeField] private float impulseForce; // The strength of the impulse

    private void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_BoxCollider = GetComponent<BoxCollider2D>();
        m_Rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        player = collision.gameObject.GetComponent<Character_Controller>();
        
        if (player != null)
        {
            player.GetDamage(damage);

            // Get the Rigidbody2D of the player object
            Rigidbody2D playerRigidbody = player.GetComponent<Rigidbody2D>();

            if (playerRigidbody != null)
            {
                // Calculate the direction of the impulse (e.g., away from the trap)
                Vector2 impulseDirection = (gameObject.transform.position - transform.position).normalized;

                // Apply the impulse to the player's Rigidbody2D
                playerRigidbody.AddForce(impulseDirection * impulseForce, ForceMode2D.Impulse);
            }
            else
            {
                Debug.LogWarning(gameObject.name + " No tiene el script buscado");
                return;
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        player = null;
    }
}