using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class JumpPlatform : MonoBehaviour
{
    public float jumpForce = 10f; // Amount of force applied to the player when they jump on the platform

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Character_Controller character = collision.gameObject.GetComponent<Character_Controller>();

        // Check if the object colliding with the platform is the player
        if (character != null)
        {
            // Get the player's Rigidbody2D component
            Rigidbody2D characterRigidbody = character.GetComponent<Rigidbody2D>();

            if (characterRigidbody != null)
            {
                // Check if the player is falling or stationary before applying the jump boost
                if (characterRigidbody.velocity.y <= 0) // Only apply if falling or stationary
                {
                    // Loop through the contact points to check where the player is hitting the platform
                    foreach (ContactPoint2D contact in collision.contacts)
                    {
                        // Print the contact normal to the console to debug
                        //Debug.Log("Contact Normal: " + contact.normal);

                        // Visualize the contact point and normal in the Scene view
                        //Debug.DrawRay(contact.point, contact.normal, Color.white, 2.0f);

                        // Apply force only if the collision happens on top of the platform
                        if (contact.normal.y < -0.8f) // Normal should point mostly upwards
                        {
                            // Reset vertical velocity and apply an upward force to the player's Rigidbody2D
                            characterRigidbody.velocity = new Vector2(characterRigidbody.velocity.x, 0);
                            characterRigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                            //Debug.Log("Jump impulse applied!"); // Log when the impulse is applied
                            character.ResetAirTime();
                            break; // Only apply the force once per collision
                        }
                    }
                }
            }
        }
    }
}