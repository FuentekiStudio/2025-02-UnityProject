using UnityEngine;

public class Gate_SFX_component : MonoBehaviour
{
    [SerializeField] private gateScript gateScript;

    public AudioClip doorOpen;
    public AudioClip doorClose;

    public void PlaySFX()
    {
        if (gateScript.openedGate)
            Audio_Manager.Instance.PlaySFX(doorOpen);
        else
            Audio_Manager.Instance.PlaySFX(doorClose);
    }
}