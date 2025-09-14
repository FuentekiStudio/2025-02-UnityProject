using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowersController : MonoBehaviour
{
    private List<TDAStack<int>> towers = new List<TDAStack<int>>();
    private TDAStack<int> selectedTower;
    private int towersAmount;

    [SerializeField] private int towersMaxHeight = 1;
    [SerializeField] private GameObject towersGameObject;

    private void Awake()
    {
        towersAmount = towersGameObject.transform.childCount;
        for (int i = 0; i < towersAmount; i++)
        {
            towers.Add(new TDAStack<int>(towersMaxHeight));
        }

        for (int i = towersMaxHeight; i > 0; i--)
        {
            towers[0].Push(i);
        }

        Debug.Log(towers[0].ToString());
    }

    public void OnTowerButtonClickHandler(int index)
    {
        TDAStack<int> tower = towers[index];

        if (selectedTower == null)
        {
            SelectTower(tower);
        }
        else
        {
            PassTowerItem(tower);
        }

        if(towers[towersAmount - 1].IsFull())
        {
            //Puzzle Ends
            Debug.Log("WON");
            gameObject.SetActive(false);
        }
    }

    public void SelectTower(TDAStack<int> towerToSelect)
    {
        if (towerToSelect.IsEmpty())
        {
            Debug.Log("!! Selected tower is empty");
            return;
        }

        selectedTower = towerToSelect;
        Debug.Log("Selected tower index: " + towers.IndexOf(towerToSelect));

    }

    public void DeselectTower()
    {
        selectedTower = null;
        Debug.Log("Tower Deselected");
    }

    public void PassTowerItem(TDAStack<int> newTower)
    {
        int item = towers[towers.IndexOf(selectedTower)].Top();

        if (selectedTower == newTower)
        {
            Debug.Log("! Both towers are the same");
            DeselectTower();
            return;
        }
        if (newTower.IsFull())
        {
            Debug.Log("!! New Tower is full");
            return;
        }
        if (newTower.Top() < item && !newTower.IsEmpty())
        {
            Debug.Log("!! Item is of lesser value than the selected tower top");
            return;
        }

        selectedTower.Pop();
        newTower.Push(item);

        Debug.Log("Item (" + item + ") has been passed from tower " + towers.IndexOf(selectedTower) + " to tower " + towers.IndexOf(newTower));

        DeselectTower();
    }
}
