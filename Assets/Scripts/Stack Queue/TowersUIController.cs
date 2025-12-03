using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowersUIController : MonoBehaviour
{
    private List<GameObject> towers = new List<GameObject>();

    [SerializeField] private TowersController towersControllerRef;
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject towersParentObject;
    [SerializeField] private GameObject buttonsParentObject;
    [SerializeField] private GameObject twPefab;
    [SerializeField] private GameObject btnPefab;
    [SerializeField] private List<GameObject> itemsList = new List<GameObject>();



    private void Awake()
    {
        for (int i = 0; i < towersControllerRef.TowersAmount; i++)
        {
            //Set up each tower
            GameObject newTower = Instantiate(twPefab);
            newTower.transform.SetParent(towersParentObject.transform);
            towers.Add(newTower);

            //Set up each button
            GameObject newButton = Instantiate(btnPefab);
            newButton.transform.SetParent(buttonsParentObject.transform);

            Button tempButton = newButton.GetComponent<Button>();
            int tempInt = i;

            tempButton.onClick.AddListener(() => towersControllerRef.OnTowerButtonClickHandler(tempInt));
            //tempButton.onClick.AddListener(() => OnTowerButtonClickHandler(tempInt));

        }

        for (int i = 0; i < towersControllerRef.TowersMaxHeight; i++)
        {
            //Debug.Log(i);
            GameObject item = Instantiate(itemsList[i]);
            item.transform.SetParent(towers[towersControllerRef.StartTowerIndex].transform);
        }



        canvas.SetActive(false);
    }


    public void MoveItemByTowerIndex(int selectedTower, int newTower)
    {
        Transform item = towers[selectedTower].transform.GetChild(0);
        item.SetParent(towers[newTower].transform);
        item.SetAsFirstSibling();
    }

    public void ShowTower()
    {
        canvas.SetActive(true);
        towersControllerRef.PlayerRef.enabled = false;
    }


    //public void OnTowerButtonClickHandler(int index)
    //{
    //    Debug.Log(towersControllerRef.GetSelectedTowerIndex());
    //    Transform item = towers[towersControllerRef.GetSelectedTowerIndex()].transform.GetChild(0);
    //    item.SetParent(towers[index].transform);
    //    item.SetAsFirstSibling();
    //}
}
