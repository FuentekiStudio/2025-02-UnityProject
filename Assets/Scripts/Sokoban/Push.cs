using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Push : MonoBehaviour
{
    private GameObject[] Obstacles; // Array to store objects tagged as Obstacles that block movement
    private GameObject[] ObjectsToPush; // Array to store objects tagged as ObjectsToPush (other pushable objects)

    void Start()
    {
        // Find all objects tagged as "Obstacles" and store them in the Obstacles array
        Obstacles = GameObject.FindGameObjectsWithTag("Obstacles");

        // Find all objects tagged as "ObjectsToPush" and store them in the ObjectsToPush array
        ObjectsToPush = GameObject.FindGameObjectsWithTag("ObjectsToPush");
    }

    // Method to move the pushable object in the specified direction
    public bool Move(Vector2 direction)
    {
        // Check if the movement is blocked by an obstacle or another pushable object
        if (ObjToBlock(transform.position, direction))
        {
            return false; // If blocked, the pushable object doesn't move
        }
        else
        {
            transform.Translate(direction); // If not blocked, move the object
            return true; // Return true to indicate successful movement
        }
    }

    // Method to check if the pushable object's path is blocked
    public bool ObjToBlock(Vector3 position, Vector2 direction)
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

        // Check if the new position collides with another pushable object
        foreach (var objToPush in ObjectsToPush)
        {
            if (objToPush.transform.position.x == newpos.x && objToPush.transform.position.y == newpos.y)
            {
                return true; // Blocked by another pushable object
            }
        }

        return false; // Return false if nothing blocks the way
    }
}
