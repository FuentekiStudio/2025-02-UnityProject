using UnityEngine;

public class Character_SFX_component : MonoBehaviour
{
    public AudioClip[] stepsClip;

    public void FootStepsSFX()
    {
        AudioClip clip;

        for (int i = 0; i < stepsClip.Length; i++)
        {
            int random = Random.Range(0, stepsClip.Length);

            clip = stepsClip[random];
            Audio_Manager.Instance.PlaySFX(clip);
        }
    }
}