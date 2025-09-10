using UnityEngine;

public class UI_Manager_SFX_component : MonoBehaviour
{
    public AudioClip toggleMenu;

    public void PlayToggleMenuSFX()
    {
        Audio_Manager.Instance.PlaySFX(toggleMenu);
    }
}