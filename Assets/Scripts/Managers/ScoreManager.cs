using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;
    private void Awake()
    {
        // Check if instance already exists
        if (instance == null)
        {
            // If not, set this instance
            instance = this;
        }
        else
        {
            // If an instance already exists, destroy this one
            Destroy(gameObject);
        }
    }

    public Dictionary<ItemData, InventoryItem> inventoryDictionaryScore;

    public int inventoryItemsCount;


    public void Update()
    {
        if (inventoryDictionaryScore == null)
        {
            return;
        }
        inventoryItemsCount = inventoryDictionaryScore.Count;
    }


    public void SaveInventory(Dictionary<ItemData, InventoryItem> inventory)
    {
        inventoryDictionaryScore = new Dictionary<ItemData, InventoryItem>();
        foreach (var item in inventory)
        {
            inventoryDictionaryScore.Add(item.Key, new InventoryItem(item.Key, item.Value.stackSize));
        }
    }

    public Dictionary<ItemData, InventoryItem> LoadInventory()
    {
        if (inventoryDictionaryScore == null)
        {
            return new Dictionary<ItemData, InventoryItem>();
        }
        return inventoryDictionaryScore;
    }

    public void ResetInventory(){
       inventoryDictionaryScore = new Dictionary<ItemData, InventoryItem>();
    }

}
