using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapArrow : MonoBehaviour
{
    [SerializeField] float detectionRange = 10f;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] Vector2 rayDirection = Vector2.right;
    [SerializeField] float arrowSpeed = 500f;
    [SerializeField] float arrowDamage = 50f;  // Amount of damage arrows will deal to the player

    private Rigidbody2D arrowRigidbody;
    private Rigidbody2D arrowBelowRigidbody;
    private SpriteRenderer arrowSpriteRenderer;
    private SpriteRenderer arrowBelowSpriteRenderer;
    private Material arrowMaterial;
    private Material arrowBelowMaterial;

    private bool trapActivated = false;

    private void Start()
    {
        arrowRigidbody = transform.Find("Arrow").GetComponent<Rigidbody2D>();
        arrowBelowRigidbody = transform.Find("Arrow_below").GetComponent<Rigidbody2D>();
        arrowSpriteRenderer = transform.Find("Arrow").GetComponent<SpriteRenderer>();
        arrowBelowSpriteRenderer = transform.Find("Arrow_below").GetComponent<SpriteRenderer>();

        // Get the materials used by the arrows
        arrowMaterial = arrowSpriteRenderer.material;
        arrowBelowMaterial = arrowBelowSpriteRenderer.material;

        arrowRigidbody.isKinematic = true;
        arrowBelowRigidbody.isKinematic = true;

        // Check and handle sprite flip based on the ray direction
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

        arrowRigidbody.isKinematic = false;
        arrowBelowRigidbody.isKinematic = false;

        // Fire both arrows by applying force and enabling their damage logic
        FireArrow(arrowRigidbody);
        FireArrow(arrowBelowRigidbody);

        // Highlight arrows by modifying the material's outline properties
        HighlightArrows();
    }

    private void FireArrow(Rigidbody2D arrow)
    {
        // Apply force to the arrow in the direction specified
        arrow.AddForce(rayDirection * arrowSpeed);

        // Attach the Arrow script to manage damage when it hits the player
        Arrow arrowScript = arrow.GetComponent<Arrow>();
        if (arrowScript != null)
        {
            arrowScript.SetDamage(arrowDamage);
        }
    }

    // Flip the arrows if the rayDirection is to the left
    private void FlipArrowsIfNecessary()
    {
        // If rayDirection is pointing left (rayDirection.x < 0), flip the arrow's sprite
        bool flip = rayDirection.x < 0;

        // Flip the arrow sprite along the X-axis if needed
        arrowSpriteRenderer.flipX = flip;
        arrowBelowSpriteRenderer.flipX = flip;
    }

    private void HighlightArrows()
    {
        // Adjust the shader properties to highlight the arrows
        arrowMaterial.SetColor("_OutlineColor", Color.yellow); // Set a brighter color
        arrowMaterial.SetFloat("_OutlineThickness", 0.1f);     // Increase the thickness

        arrowBelowMaterial.SetColor("_OutlineColor", Color.yellow); // Set a brighter color
        arrowBelowMaterial.SetFloat("_OutlineThickness", 0.1f);     // Increase the thickness

        // Optional: Restore the original properties after a delay
        Invoke(nameof(RestoreOriginalGlow), 1f); // Restore after 1 second
    }

    private void RestoreOriginalGlow()
    {
        // Restore the original outline color and thickness
        arrowMaterial.SetColor("_OutlineColor", Color.white);
        arrowMaterial.SetFloat("_OutlineThickness", 0.03f); // Original thickness

        arrowBelowMaterial.SetColor("_OutlineColor", Color.white);
        arrowBelowMaterial.SetFloat("_OutlineThickness", 0.03f); // Original thickness
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)rayDirection * detectionRange);
    }
}