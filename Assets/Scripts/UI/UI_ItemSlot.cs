using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Handles the UI representation of an individual item slot in the inventory.
/// This script updates the item's icon and stack count displayed in the UI.
/// </summary>
public class UI_ItemSlot : MonoBehaviour
{
    // Reference to the Image component that displays the item's icon
    [SerializeField] private Image itemImage;

    // Reference to the TextMeshProUGUI component that displays the item's stack size
    [SerializeField] private TextMeshProUGUI itemText;

    // Holds the reference to the current InventoryItem associated with this slot
    public InventoryItem item;

    /// <summary>
    /// Updates the UI elements of the item slot with the provided item data.
    /// </summary>
    /// <param name="_newItem">The new item to display in this slot.</param>
    public void UpdateSlot(InventoryItem _newItem)
    {
        // Assign the new item data to the slot
        item = _newItem;

        // If the item is not null, update the UI elements
        if (item != null)
        {
            // Set the item's icon in the UI Image component
            itemImage.sprite = item.data.icon;
            itemImage.enabled = true; // Ensure the image is visible

            // Display the stack size if it is greater than 1, otherwise clear the text
            itemText.text = item.stackSize > 1 ? item.stackSize.ToString() : "";
        }
        else
        {
            // If the item is null, clear the UI elements
            itemImage.enabled = false;
            itemText.text = "";
        }
    }
}
