using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*

    Explanation of PlayerMovement:
    The script handles the player's movement in a 2D grid-based environment.
    It checks if there are obstacles or pushable objects in the direction of movement. If there's a pushable object, it attempts to push it. If blocked by an obstacle or an unmovable object, the player can't move.
    The Move() function ensures that the player only moves along a single axis and handles the actual translation of the player.
    The Blocked() function checks if the player is blocked by any obstacle or pushable object in the intended direction of movement.
    The player's movement input is managed in the Update() function using Unity's input system (Horizontal and Vertical axes).
 
 */

public class PlayerMovement : MonoBehaviour
{
    private GameObject[] Obstacles; // Array to store objects tagged as Obstacles that block the player's movement
    private GameObject[] ObjectsToPush; // Array to store objects tagged as ObjectsToPush that the player can move by pushing

    private bool readyToMove; // Flag to check if the player is ready to move again after completing a movement

    // Start is called before the first frame update
    void Start()
    {
        // Find all objects tagged as "Obstacles" and store them in the Obstacles array
        Obstacles = GameObject.FindGameObjectsWithTag("Obstacles");

        // Find all objects tagged as "ObjectsToPush" and store them in the ObjectsToPush array
        ObjectsToPush = GameObject.FindGameObjectsWithTag("ObjectsToPush");
    }

    // Update is called once per frame
    void Update()
    {
        // Get input from player for horizontal and vertical movement (arrow keys or WASD)
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        moveInput.Normalize(); // Normalize the vector so that diagonal movement isn't faster than single-axis movement

        // Check if there's any movement input
        if (moveInput.sqrMagnitude > 0.5)
        {
            if (readyToMove) // Only allow movement if the player is ready to move
            {
                readyToMove = false; // Disable further movement until the player is ready again
                Move(moveInput); // Call the Move function with the player's movement direction
            }
        }
        else
        {
            readyToMove = true; // Reset the readyToMove flag when no input is detected
        }
    }

    // Method to handle movement in a specified direction
    public bool Move(Vector2 direction)
    {
        // Ensure the movement is along one axis (no diagonal movement)
        if (Mathf.Abs(direction.x) < 0.5)
        {
            direction.x = 0;
        }
        else
        {
            direction.y = 0;
        }
        direction.Normalize(); // Normalize the direction vector

        // Check if the movement is blocked by obstacles or pushable objects
        if (Blocked(transform.position, direction))
        {
            return false; // If blocked, don't move
        }
        else
        {
            transform.Translate(direction); // If not blocked, move the player
            return true; // Return true to indicate successful movement
        }
    }

    // Method to check if the player's path is blocked by obstacles or pushable objects
    public bool Blocked(Vector3 position, Vector2 direction)
    {
        // Calculate the new position based on the current position and movement direction
        Vector2 newpos = new Vector2(position.x, position.y) + direction;

        // Check if the new position collides with any obstacles
        foreach (var obj in Obstacles)
        {
            if (obj.transform.position.x == newpos.x && obj.transform.position.y == newpos.y)
            {
                return true; // Blocked by an obstacle
            }
        }

        // Check if the new position collides with any pushable objects
        foreach (var objToPush in ObjectsToPush)
        {
            if (objToPush.transform.position.x == newpos.x && objToPush.transform.position.y == newpos.y)
            {
                // If it's a pushable object, attempt to push it in the same direction
                Push objPush = objToPush.GetComponent<Push>(); // Get the Push component from the object
                if (objPush && objPush.Move(direction)) // If it can be pushed, return false (not blocked)
                {
                    return false;
                }
                else
                {
                    return true; // If it cannot be pushed, return true (blocked)
                }
            }
        }

        return false; // Return false if no obstacles or pushable objects block the way
    }
}
