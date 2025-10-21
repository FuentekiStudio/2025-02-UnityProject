using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private ArrowData _arrowData;
    public ArrowPool poolRef;

    [SerializeField] private Rigidbody2D _rb;

    private float _currentDamage;
    private bool _isFired = false;
    public bool IsFired => _isFired;

    private void Start()
    {
        if (_rb == null)
            _rb = GetComponent<Rigidbody2D>();

        if (poolRef == null)
            poolRef = GetComponentInParent<ArrowPool>();

        _currentDamage = _arrowData.damage;
    }

    private void Update()
    {
        if (_isFired)
        {
            if (this.transform.position.x > 100f)
            {
                poolRef.arrowPool.Release(this);
                poolRef.arrowPool.Get();
                _isFired = false;
            }
        }
    }

    public void SetDamage(float damageAmount)
    {
        _currentDamage = damageAmount;
        _isFired = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Character_Controller player = collision.GetComponent<Character_Controller>();
        if (player && _isFired)
        {
            player.GetDamage(_currentDamage);
            //Debug.Log("Player hit by the arrow!");

            poolRef.arrowPool.Release(this);
            poolRef.arrowPool.Get();
            _isFired = false;
        }
    }

    public ArrowData GetArrowData() => _arrowData;
    public Rigidbody2D GetRigidbody2D() => _rb;
}
