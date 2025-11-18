using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcessDataComponent
{

    private LevelAndInventoryData data = null;

    public LevelAndInventoryData Data
    {
        get { return data; }
        set { data = value; }
    }


    public int GetLevel()
    {
        return data.savedLevel;

    }

    public int[] GetInventoryStacks()
    {
        return data.objectStack;
    }

    public string[] GetInventoryIDs()
    {
        return data.objectNames;
    }
}
