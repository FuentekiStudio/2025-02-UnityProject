using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class LevelAndInventoryData
{
    public int savedLevel;
    public int[] objectStack;
    public string[] objectNames;

    public LevelAndInventoryData(int sceneIndex, string[] itemNames, int[] itemStacks)
    {
        savedLevel = sceneIndex;
        objectNames = itemNames;
        objectStack = itemStacks;


        Debug.Log($"Nivel guardado en LevelInventoryData {savedLevel}");
    }

    //public static LevelAndInventoryData ReturnClass()
    //{
    //    return new LevelAndInventoryData
    //    {

    //        savedLevel = 1,
    //        objectStack = new int[] { 1, 2, 3 }

    //    };

    //}
}
