using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TowersController : MonoBehaviour
{
    private List<TDAStack<int>> towers = new List<TDAStack<int>>();
    private TDAStack<int> selectedTower;
    [SerializeField] private int towersAmount;
    [SerializeField] private int towersMaxHeight = 1;
    [SerializeField] private int startTowerIndex = 0;

    private TowersUIController uiController;

    public TDAStack<int> SelectedTower => selectedTower;
    public int TowersAmount => towersAmount;
    public int TowersMaxHeight => towersMaxHeight;
    public int StartTowerIndex => startTowerIndex;

    public UnityEvent onCompletedGame;

    private void Awake()
    {
        uiController = gameObject.GetComponent<TowersUIController>();

        for (int i = 0; i < towersAmount; i++)
        {
            towers.Add(new TDAStack<int>(towersMaxHeight));
        }

        FillStartingTower();

        Debug.Log(towers[startTowerIndex].ToString());
    }

    private void FillStartingTower()
    {
        for (int i = towersMaxHeight; i > 0; i--)
        {
            towers[startTowerIndex].Push(i);
        }
    }

    public void OnTowerButtonClickHandler(int index)
    {
        Debug.Log(index);
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
            SendEvent();
            gameObject.SetActive(false);
        }
    }

    private void SelectTower(TDAStack<int> towerToSelect)
    {
        if (towerToSelect.IsEmpty())
        {
            Debug.Log("!! Selected tower is empty");
            return;
        }

        selectedTower = towerToSelect;
        Debug.Log("Selected tower index: " + towers.IndexOf(towerToSelect));

    }

    private void DeselectTower()
    {
        selectedTower = null;
        Debug.Log("Tower Deselected");
    }

    private void PassTowerItem(TDAStack<int> newTower)
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

        uiController.MoveItemByTowerIndex(towers.IndexOf(selectedTower), towers.IndexOf(newTower));

        selectedTower.Pop();
        newTower.Push(item);

        Debug.Log("Item (" + item + ") has been passed from tower " + towers.IndexOf(selectedTower) + " to tower " + towers.IndexOf(newTower));

        DeselectTower();
    }

    public int GetSelectedTowerIndex()
    {
        //Debug.Log(towers.IndexOf(selectedTower) + 1);
        return towers.IndexOf(selectedTower) + 1;
    }

    private void SendEvent()
    {
        onCompletedGame?.Invoke();
        PlayerController.instance.enabled = true;
    }
}
