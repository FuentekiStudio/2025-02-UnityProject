using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class DataStorageComponent
{
    private string path;

    public DataStorageComponent(string p)
    {
        path = p;
    }

    public void Save(LevelAndInventoryData saveData)
    {
        string json = JsonUtility.ToJson(saveData, true);

        File.WriteAllText(path, json);
        
    }

    public LevelAndInventoryData Load()
    {

        if (!File.Exists(path)) return null;
        string jsonFromFile = File.ReadAllText(path);
        try { return JsonUtility.FromJson<LevelAndInventoryData>(jsonFromFile); }
        catch { return null; }
        
    }

}
