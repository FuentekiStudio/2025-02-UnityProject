using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Inventory : MonoBehaviour
{

    public static Inventory instance;

    public List<InventoryItem> inventoryItems;
    public Dictionary<ItemData, InventoryItem> inventoryDictionary;

    // New: Dictionary to track items by ID
    private Dictionary<int, int> itemTypeCounts;

    [Header("Inventory UI")]
    [SerializeField] private Transform inventorySlotParent;
    private UI_ItemSlot[] itemSlot;



    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

    }

    private void Start()
    {
        inventoryDictionary = new Dictionary<ItemData, InventoryItem>();
        foreach (var item in ScoreManager.instance.LoadInventory())
        {
            inventoryDictionary.Add(item.Key, new InventoryItem(item.Key, item.Value.stackSize));
        }


        inventoryItems = new List<InventoryItem>();
        InitList(inventoryDictionary);

        // Initialize itemTypeCounts
        itemTypeCounts = new Dictionary<int, int>();

        itemSlot = inventorySlotParent.GetComponentsInChildren<UI_ItemSlot>();
        UpdateSlotUI();
    }

    private void UpdateSlotUI()
    {
        if (itemSlot == null || itemSlot.Length == 0)
        {
            Debug.LogError("No se han encontrado slots de inventario. Aseg�rate de que inventorySlotParent tiene UI_ItemSlot como hijos.");
            return;
        }


        for (int i = 0; i < inventoryItems.Count; i++)
        {
            itemSlot[i].UpdateSlot(inventoryItems[i]);
        }
    }

    private void InitList(Dictionary<ItemData, InventoryItem> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            inventoryItems.Add(list.ElementAt(i).Value);
        }
    }

    public void AddItem(ItemData _item)
    {
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            inventoryItems.Add(newItem);
            inventoryDictionary.Add(_item, newItem);
        }

        // Update the count of the item type by ID
        if (itemTypeCounts.ContainsKey(_item.ID))
        {
            itemTypeCounts[_item.ID]++;
        }
        else
        {
            itemTypeCounts[_item.ID] = 1;
        }

        UpdateSlotUI();
    }

    public void RemoveItem(ItemData _item)
    {
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            if (value.stackSize <= 1)
            {
                inventoryItems.Remove(value);
                inventoryDictionary.Remove(_item);
            }
            else
            {
                value.RemoveStack();
            }

            // Update the count of the item type by ID
            if (itemTypeCounts.ContainsKey(_item.ID))
            {
                itemTypeCounts[_item.ID]--;
                if (itemTypeCounts[_item.ID] <= 0)
                {
                    itemTypeCounts.Remove(_item.ID);
                }
            }
        }
        UpdateSlotUI();
    }

    // New: Get the count of items by ID
    public int GetItemCountByID(int id)
    {
        if (itemTypeCounts.TryGetValue(id, out int count))
        {
            return count;
        }
        return 0; // Return 0 if the item type is not found
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.L))
    //    {
    //        ItemData newItem = inventoryItems[inventoryItems.Count - 1].data;
    //        Debug.Log("Item Removido del inventario: " + newItem.itemName);
    //        RemoveItem(newItem);
    //    }
    //}
}
