using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveSystem
{
    private string path;


    private DataStorageComponent dataStorage;
    private ProcessDataComponent processData;

    public SaveSystem()
    {
        path = Application.persistentDataPath + "/playerData.json";
        dataStorage = new DataStorageComponent(path);
        processData = new ProcessDataComponent();
    }


    public void SaveData(LevelAndInventoryData playerData)
    {
        dataStorage.Save(playerData);
    }


    public int GetSavedLevel()
    {
        if (processData.Data == null)
        {
            LoadData();
        }

        return processData.GetLevel();
    }
    
    public int[] GetSavedInventory()
    {
        if (processData.Data == null)
        {
            LoadData();
        }

        return processData.GetInventoryStacks();
    }

    private void LoadData()
    {
        processData.Data = dataStorage.Load();
    }
}
