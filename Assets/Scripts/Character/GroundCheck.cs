using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public bool grounded=true;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.isTrigger == false)
        {
            grounded = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.isTrigger == false)
        {
            grounded = false;
        }
    }
}
