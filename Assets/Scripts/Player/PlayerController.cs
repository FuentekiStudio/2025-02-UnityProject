using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Party Management
    [SerializeField] private PartyHandler _partyHandler;
    public PartyHandler partyHandler => _partyHandler;
    [SerializeField] private Character_Controller _selectedOne;
    public Character_Controller selectedOne { get { return _selectedOne; } }

    [SerializeField] private Inventory _playerInventory;

    public Inventory PlayerInventory { get { return _playerInventory; } }
   
    public int coins = 0;
    
    /// <Events>
    
    //public delegate void OnSelectedOneHandler(Character_Controller new_SelectedOne);
    //public event OnSelectedOneHandler OnSelectedOneChange;

    public delegate void OnCoinsUpdatedHandler(int coins);
    public event OnCoinsUpdatedHandler OnCoinsUpdated;
    
    /// </Events> 

   

    // Start is called before the first frame update
    void Start()
    {
        if (_partyHandler._characterPool == null)
        {
            Debug.LogWarning("character pool is empty");
        }
        else
        {
            _selectedOne = _partyHandler.selectedOne;
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
        if (_partyHandler._characterPool == null)
        {
            Debug.LogWarning("character pool is empty");
            return;
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            _selectedOne = _partyHandler.SelectCharacter(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _selectedOne = _partyHandler.SelectCharacter(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _selectedOne = _partyHandler.SelectCharacter(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _selectedOne = _partyHandler.SelectCharacter(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            _selectedOne = _partyHandler.SelectCharacter(4);
        }
    }

    private void MoveCharacter()
    {
        if (_selectedOne == null)
            return;

        //_selectedOne.UpdateStateMachine();

        _selectedOne.HorizontalAxis(Input.GetAxisRaw("Horizontal"));
        _selectedOne.jumping = Input.GetKeyDown(KeyCode.Space);

    }

    private void Interaction()
    {
        if (_selectedOne == null)
            return;
        if (Input.GetKeyDown(KeyCode.E))
        {
            //Debug.Log("try to interact");
            _selectedOne.InteractWithObject();
        }
    }

    private void HealthMod()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            _selectedOne.Healing(1);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            _selectedOne.GetDamage(1);
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
    public void AddItemToInventory(ItemData data)
    {
       // coins++;
        //OnCoinsUpdated?.Invoke(coins);
        _playerInventory.AddItem(data);
    }

}
