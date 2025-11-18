using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveHandler
{
    private SaveSystem saveSystem;

    string[] itemNames;
    int[] itemStacks;

    public void Initialize()
    {
        saveSystem = new SaveSystem();
    }

    public void SaveGame()
    {
        Debug.Log("Guarda (ojo)");
        Inventory playerInv = GameManager.instanceGM.PlayerInventory;
        itemNames = new string[playerInv.inventoryItems.Count];
        itemStacks = new int[playerInv.inventoryItems.Count];

        int count = 0;
        foreach (InventoryItem item in playerInv.inventoryItems)
        {
            itemNames[count] = item.data.itemName;
            itemStacks[count] = item.stackSize;
            Debug.Log($"Item guardado: {itemNames[count]}, {itemStacks[count]}");
            count++;
        }

        LevelAndInventoryData data = new LevelAndInventoryData(Scene_Manager.Instance.currentSceneIndex + 1, itemNames, itemStacks);
        saveSystem.SaveData(data);
    }

    public void LoadGame()
    {
        Debug.Log("Carga");

        Debug.Log(saveSystem.GetSavedLevel());
        Scene_Manager.Instance.LoadSceneWithLoadingScreen(saveSystem.GetSavedLevel());
    }

    public void LoadInventory(Inventory inv)
    {
        itemNames = saveSystem.GetSavedInventoryIDs();
        itemStacks = saveSystem.GetSavedInventoryStacks();

        for (int i = 0; i < itemNames.Length; i++)
        {
            ItemData newData = Resources.Load<ItemData>($"ScriptableObjects/Items/{itemNames[i]}");
            //Debug.Log($"ScriptableObjects/Items/{itemNames[i]}");
            //Debug.Log(newData);
            inv.AddItem(newData, itemStacks[i]);
        }
    }
}
