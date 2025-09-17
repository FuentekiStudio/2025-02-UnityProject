using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalPlatform : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody2D playerRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();


            if (playerRigidbody != null)
            {
                foreach (ContactPoint2D contact in collision.contacts)
                {
                    // Print the contact normal to the console to debug
                   // Debug.Log("Contact Normal Platform: " + contact.normal);

                    // Visualize the contact point and normal in the Scene view
                    Debug.DrawRay(contact.point, contact.normal, Color.white, 2.0f);

                    // Apply force only if the collision happens on top of the platform
                    if (contact.normal.y < -0.8f) // Normal should point mostly upwards
                    {
                        // Zero out the player's horizontal velocity to stop movement when touching the platform
                        Vector2 currentVelocity = playerRigidbody.velocity;
                        playerRigidbody.velocity = new Vector2(0, currentVelocity.y); // Stop horizontal movement but keep vertical velocity
                    }
                }

            }
        }
    }
}
