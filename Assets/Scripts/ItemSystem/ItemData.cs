using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

/// <summary>
/// Represents the data associated with a collectible item in the game.
/// This class is used as a data container to define item properties such as name and icon.
/// Utilizes Unity's ScriptableObject system for efficient data management.
/// </summary>
/// 

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Item")]
public class ItemData : ScriptableObject
{
    private void OnValidate()
    {
        if (string.IsNullOrEmpty(itemName))
        {
            Debug.LogWarning("Item name is missing in " + name);
        }

        if (icon == null)
        {
            Debug.LogWarning("Icon is missing in " + name);
        }
    }

    // The display name of the item, shown in the game's UI
    [Tooltip("The name of the item")]
    public string itemName;

    // The icon associated with the item, used for displaying in the inventory UI
    [Tooltip("The icon representing the item in the UI")]
    public Sprite icon;

    [Tooltip("The number identification to difference items")]
    public int ID;
}

