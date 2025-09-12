using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowersController : MonoBehaviour
{
    private List<TDAStack<int>> towers = new List<TDAStack<int>>();
    [SerializeField] private int towersMaxHeight = 1;
    private int selectedTowerIndex;

    private void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            towers.Add(new TDAStack<int>(towersMaxHeight));
        }

        for (int i = towersMaxHeight; i > 0; i--)
        {
            towers[0].Push(i);
        }

        Debug.Log(towers[0].ToString());
        Debug.Log(towers[1].ToString());
        Debug.Log(towers[2].ToString());
    }


}
