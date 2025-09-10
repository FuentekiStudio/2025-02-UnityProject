using UnityEngine;

public class gateScript : MonoBehaviour
{
    [SerializeField] private Gate_SFX_component gate_SFX;

    public bool openedGate = false;

    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Sprite openDoor;
    [SerializeField] Sprite closeDoor;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void ChangeSprite()
    {
        openedGate = !openedGate;

        if (openedGate)
        {
            Debug.Log("puerta abierta");
            gate_SFX.PlaySFX();
            spriteRenderer.sprite = openDoor;
        }
        else
        {
            Debug.Log("puerta cerrada");
            gate_SFX.PlaySFX();
            spriteRenderer.sprite = closeDoor;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Character_Controller>() != null && openedGate)
        {
            GameManager.instanceGM.showSR = true;
        }
    }
}
