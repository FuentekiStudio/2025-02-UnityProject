using UnityEngine;

public class TrapArrow : MonoBehaviour
{
    [SerializeField] private Factory _factory;

    [SerializeField] float detectionRange = 10f;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] Vector2 rayDirection = Vector2.right;
    [SerializeField] float arrowSpeed = 500f;
    [SerializeField] float arrowDamage = 50f;

    [Header("Arrow references")]
    [SerializeField] private Arrow _arrow;
    [SerializeField] private Rigidbody2D _arrowRigidbody;
    [SerializeField] private SpriteRenderer _arrowSpriteRenderer;
    [SerializeField] private Material _arrowMaterial;

    private bool trapActivated = false;

    private void Start()
    {
        if (this.GetComponentInChildren<Arrow>() == null)
        {
            _arrow = _factory.CreateArrow("Arrow");
            _arrowRigidbody = _arrow.GetComponent<Rigidbody2D>();

            if (_arrowRigidbody != null)
                _arrowRigidbody.isKinematic = true;

            _arrowSpriteRenderer = _arrow.GetComponent<SpriteRenderer>();

            if (_arrowSpriteRenderer != null)
                _arrowMaterial = _arrowSpriteRenderer.material;
        }
        else
        {
            if (_arrow == null)
            {
                _arrow = GetComponentInChildren<Arrow>();
            }
                
            if (_arrowRigidbody == null)
            {
                _arrowRigidbody = _arrow.GetComponent<Rigidbody2D>();
                _arrowRigidbody.isKinematic = true;
            }
            else
            {
                _arrowRigidbody.isKinematic = true;
            }

            if (_arrowSpriteRenderer == null)
            {
                _arrowSpriteRenderer = _arrow.GetComponent<SpriteRenderer>();
                _arrowMaterial = _arrowSpriteRenderer.material;
            }
            else
            {
                _arrowMaterial = _arrowSpriteRenderer.material;
            }
        }

        FlipArrowsIfNecessary();
    }

    private void Update()
    {
        if (!trapActivated)
        {
            CastRay();
        }
    }

    private void CastRay()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection, detectionRange, playerLayer);

        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            trapActivated = true;
            ActivateTrap();
        }
    }

    private void ActivateTrap()
    {
        Debug.Log("Trap activated, firing arrows!");

        _arrowRigidbody.isKinematic = false;

        FireArrow(_arrowRigidbody);

        HighlightArrows();
    }

    private void FireArrow(Rigidbody2D arrow)
    {
        arrow.AddForce(rayDirection * arrowSpeed);

        Arrow arrowScript = arrow.GetComponent<Arrow>();
        if (arrowScript != null)
        {
            arrowScript.SetDamage(arrowDamage);
        }
    }

    private void FlipArrowsIfNecessary()
    {
        bool flip = rayDirection.x < 0;
        _arrowSpriteRenderer.flipX = flip;
    }

    private void HighlightArrows()
    {
        _arrowMaterial.SetColor("_OutlineColor", Color.yellow);
        _arrowMaterial.SetFloat("_OutlineThickness", 0.1f);

        Invoke(nameof(RestoreOriginalGlow), 1f);
    }

    private void RestoreOriginalGlow()
    {
        _arrowMaterial.SetColor("_OutlineColor", Color.white);
        _arrowMaterial.SetFloat("_OutlineThickness", 0.03f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)rayDirection * detectionRange);
    }
}