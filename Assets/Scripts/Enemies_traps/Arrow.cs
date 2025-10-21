using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private ArrowData _arrowData;
    [SerializeField] private Rigidbody2D rb;

    private float _currentDamage;
    private bool isFired = false;

    private void Start()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        _currentDamage = _arrowData.damage;
    }

    public void SetDamage(float damageAmount)
    {
        _currentDamage = damageAmount;
        isFired = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Character_Controller player = collision.GetComponent<Character_Controller>();
        if (player && isFired)
        {
            player.GetDamage(_currentDamage);
            //Debug.Log("Player hit by the arrow!");

            Destroy(gameObject);
        }
    }

    public ArrowData GetArrowData() => _arrowData;
}
