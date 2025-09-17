using UnityEngine;

public class ActiveTrigger : MonoBehaviour
{
    private TrapPikes trapPikes;

    private void Start()
    {
        trapPikes = GetComponentInParent<TrapPikes>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Character_Controller>())
        {
            if (trapPikes != null)
            {
                trapPikes.m_Animator.SetTrigger("Activate");
            }
            else
            {
                Debug.LogWarning("TrapPikes reference is missing in the ActiveTrigger script.");
            }
        }
    }
}