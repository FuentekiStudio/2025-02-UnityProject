using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private Coin_SFX_component sfx;

    private void Start()
    {
        sfx = GetComponent<Coin_SFX_component>();
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            Character_Controller character = collision.GetComponent<Character_Controller>();
            //character.AddCoin();

            sfx.PlaySFX();

            this.gameObject.SetActive(false);
        }
    }

}
