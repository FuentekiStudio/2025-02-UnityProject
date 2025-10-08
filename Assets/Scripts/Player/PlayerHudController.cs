using UnityEngine;
using UnityEngine.UI;
using TMPro;



/// <summary>
/// Observer pattern
/// 
/// playerHudController Observs PlayerController and Character_controller
/// by subscribing to their Events, playerHudController knows when to update
/// 
/// player events are subscriben on awake
/// character subscriptions are updated every time a new character is selected
///     unsuscribe the old One, suscribe to new One
/// </summary>
public class PlayerHudController : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    private Character_Controller character_to_display;

    // Player HUD
    [Header("Player HUD")]
    [SerializeField] private GameObject playerHUDPanel;
    [SerializeField] private TextMeshProUGUI characterName;
    [SerializeField] private Image characterIcon;
    [SerializeField] private Slider characterHPSlider;
    [SerializeField] private TextMeshProUGUI coinCount;


    void Awake()
    {
        //subscribe to player events
        if (player != null)
        {
            player.partyHandler.OnSelectedOneChange += UpdateCharacter;
            player.OnCoinsUpdated += UpdateCoinsCount;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDestroy()
    {
        //unsusbribe to player events
        if (player != null)
        {
            player.partyHandler.OnSelectedOneChange -= UpdateCharacter;
            player.OnCoinsUpdated -= UpdateCoinsCount;
        }
    }


    //utilities functions
    public void SettingHPSlider(float value, float maxValue)
    {
        characterHPSlider.maxValue = maxValue;
        characterHPSlider.value = value;

    }
    public void UpdateSliderValue(float value)
    {
        characterHPSlider.value = value;
    }
    public void UpdateCurrentCharacterName(string name)
    {
        characterName.text = name;
    }
    public void UpdateCharacterIcon(float R, float G, float B, float A)
    {
        characterIcon.sprite = null;
        Color color = new(R, G, B, A);
        characterIcon.color = color;
    }
    public void UpdateCharacterIcon(Sprite protrait)
    {
        Color color = new(255, 255, 255, 255);
        characterIcon.color = color;
        characterIcon.sprite = protrait;
    }

    public void UpdateCoinsCount(int value)
    {
        coinCount.text = value.ToString();
    }

    //character subscriptions functions
    public void SubscribeToCharacter(Character_Controller character)
    {
        if (character == null)
        {
            return;
        }
        character.OnLifeChange += UpdateSliderValue;

    }
    public void UnSubscribeToCharacter(Character_Controller character)
    {
        if (character == null)
        {
            return;
        }
        character.OnLifeChange -= UpdateSliderValue;
    }



    private void UpdateCharacter(Character_Controller new_character_to_display)
    {
        if (new_character_to_display == null)
        {
            return;
        }

        //character related subscriptions
        UnSubscribeToCharacter(character_to_display);
        character_to_display = new_character_to_display;
        SubscribeToCharacter(character_to_display);

        Character_data data = character_to_display.GetComponent<Character_data>();

        if (data != null)
        {
            UpdateCurrentCharacterName(data.character_name);
            UpdateCharacterIcon(data.portrait_sprite);
        }
        else
        {
            UpdateCurrentCharacterName(character_to_display.name);
            UpdateCharacterIcon(character_to_display.spriteRenderer.color.r, character_to_display.spriteRenderer.color.g, character_to_display.spriteRenderer.color.b, character_to_display.spriteRenderer.color.a);
        }
        SettingHPSlider(character_to_display.currentHealth, character_to_display.maxHealth);
        UpdateSliderValue(character_to_display.currentHealth);


    }

}
