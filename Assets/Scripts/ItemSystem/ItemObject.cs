/*
 * ItemObject.cs
 * 
 * Description:
 * This script defines the behavior of an item object within the game world. Each item is linked to an ItemData ScriptableObject that holds its
 * characteristics (e.g., name and icon). This item can be picked up by the player, which will add the item to the Inventory and deactivate the item
 * in the scene.
 *
 * Architecture and Design:
 * - This script follows a modular, data-driven design approach. It uses ScriptableObjects (ItemData) to manage item properties, keeping data and 
 *   behavior separate.
 * - The "OnValidate" method allows the item to be automatically configured in the editor based on the ItemData, which helps ensure consistency 
 *   across instances.
 * - The "OnTriggerEnter2D" method detects when the player collides with the item, automatically adding it to the inventory and deactivating 
 *   the item in the scene. This approach is efficient for simple pickup items but could be extended for more complex interactions.
 * 
 * Suggestions for Improvement:
 * - Consider adding null checks for `itemData` to avoid potential issues if the data is missing.
 * - For better encapsulation and readability, separate item collection and inventory update logic into their own methods.
 * - It could be beneficial to add customizable effects (e.g., sound, particle) on item pickup for a more engaging player experience.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    // Reference to the SpriteRenderer component, used to display the item's icon.
    private SpriteRenderer sr;

    // Reference to the ItemData ScriptableObject, which contains the item's data (e.g., name, icon).
   public ItemData itemData;

    /// <summary>
    /// This method is called by Unity when the script is loaded or a value is changed in the Inspector.
    /// OnValidate automatically updates the item's sprite and name in the editor based on the linked ItemData.
    /// </summary>
    private void OnValidate()
    {
        // Assign the item's icon to the SpriteRenderer for visual representation in the scene.
        GetComponent<SpriteRenderer>().sprite = itemData.icon;

        // Set the GameObject name to include the item name for easier identification in the hierarchy.
        gameObject.name = "Item object - " + itemData.itemName;
    }

    /// <summary>
    /// This method is triggered when another Collider2D enters the item's collider area.
    /// When the player collides with the item, it is added to the inventory, and the item is deactivated.
    /// </summary>
    /// <param name="collision">The Collider2D that entered the trigger area.</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the colliding object has the "Player" tag.
        if (collision.CompareTag("Player"))
        {
            // Retrieve the Character_Controller component on the colliding player object.
            Character_Controller character = collision.GetComponent<Character_Controller>();

            // Log a message indicating the item was picked up (useful for debugging).
            Debug.Log("Picked up item: " + itemData.itemName);

            // Add the item to the inventory.
            character.AddItemToPlayerInventory(itemData);
            //Inventory.instance.AddItem(itemData);

            // Deactivate the item in the scene, simulating it being picked up.
            this.gameObject.SetActive(false);
        }
    }
}
