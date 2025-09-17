using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;


    private Character_Controller _selectedOne;
    public Character_Controller selectedOne
    {
        get { return _selectedOne; }
        set
        {
            _selectedOne = value;
            if (value != null)
            {
                OnSelectedOneChange?.Invoke(value);
            }

        }
    }
    private int selectedIndex = 0;
    public List<Character_Controller> characterPool;
    public int coins = 0;
    
    /// <Events>
    
    public delegate void OnSelectedOneHandler(Character_Controller new_SelectedOne);
    public event OnSelectedOneHandler OnSelectedOneChange;

    public delegate void OnCoinsUpdatedHandler(int coins);
    public event OnCoinsUpdatedHandler OnCoinsUpdated;
    
    /// </Events> 

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (characterPool == null)
        {
            Debug.LogWarning("character pool is empty");
        }
        else
        {
            selectedOne = characterPool[selectedIndex];
        }   
    }

    // Update is called once per frame
    void Update()
    {
        SelectCharacter();
        MoveCharacter();
        Interaction();
        HealthMod();
        PauseGame();


    }

    private void  SelectCharacter()
    {
        if (characterPool == null)
        {
            Debug.LogWarning("character pool is empty");
            return;
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SelectNext();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedOne = characterPool[0];
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectedOne = characterPool[1];
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            selectedOne = characterPool[2];
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            selectedOne = characterPool[3];
        }

    }

    private void SelectNext()
    {
        selectedIndex++;
        if (selectedIndex + 1 > characterPool.Count)
        {
            selectedIndex = 0;
        }
        selectedOne = characterPool[selectedIndex];
    }

    private void MoveCharacter()
    {
        if (selectedOne == null)
            return;

        if (Input.GetKey(KeyCode.D))
        {
            selectedOne.MoveRight();
        }

        if (Input.GetKey(KeyCode.A))
        {
            selectedOne.MoveLeft();
        }

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space))
        {
            selectedOne.Jump();  
        }
    }



    private void Interaction()
    {
        if (selectedOne == null)
            return;
        if (Input.GetKeyDown(KeyCode.E))
        {
            //Debug.Log("try to interact");
            selectedOne.InteractWithObject();
        }
    }

    private void HealthMod()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            selectedOne.Healing(1);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            selectedOne.GetDamage(1);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("Coins: " + coins);
        }
    }
    private void PauseGame()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.instanceGM.TogglePause();
        }
    }
    public void AddCoin()
    {
        coins++;
        OnCoinsUpdated?.Invoke(coins);
    }

}
