using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyHandler : MonoBehaviour
{

    [SerializeField] private Character_Controller _selectedOne;

    private int selectedIndex = 0;
    public List<Character_Controller> _characterPool;

    
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

    public delegate void OnSelectedOneHandler(Character_Controller new_SelectedOne);
    public event OnSelectedOneHandler OnSelectedOneChange;


    // Start is called before the first frame update
    void Start()
    {
        if (_characterPool == null)
        {
            Debug.LogWarning("character pool is empty");
        }
        else
        {
            selectedOne = _characterPool[selectedIndex];
        }
    }

    // Update is called once per frame
    public Character_Controller SelectCharacter(int num)
    {
        if (_characterPool == null)
        {
            Debug.LogWarning("character pool is empty");
            return null;
        }

        switch (num)
        {
            case 0:
                return SelectNext();
            case 1:
                selectedOne = _characterPool[0];
                break;
            case 2:
                selectedOne = _characterPool[1];
                break;
            case 3:
                selectedOne = _characterPool[2];
                break;
            case 4:
                selectedOne = _characterPool[3];
                break;
        }

        return selectedOne;

    }

    private Character_Controller SelectNext()
    {
        selectedIndex++;
        if (selectedIndex + 1 > _characterPool.Count)
        {
            selectedIndex = 0;
        }
        selectedOne = _characterPool[selectedIndex];

        return selectedOne;
    }

}
    
    
