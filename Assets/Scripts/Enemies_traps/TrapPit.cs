using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class TrapPit : MonoBehaviour
{
    private BoxCollider2D m_BoxCollider;


    private void Start()
    {
        m_BoxCollider = GetComponent<BoxCollider2D>();

        m_BoxCollider.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Character_Controller>())
        {
            collision.GetComponent<Character_Controller>().GetDamage(150f);
        }
        else
        {
            Debug.Log("The object collided don't have the script");
        }
    }
}
