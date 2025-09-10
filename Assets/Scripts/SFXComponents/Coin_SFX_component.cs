using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin_SFX_component : MonoBehaviour
{

    public AudioClip coinSFX;

    public void PlaySFX()
    {
        Audio_Manager.Instance.PlaySFX(coinSFX);
    }
}
