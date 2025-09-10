using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public interface Iinteractable
{
    public void ExecuteInteraction();
}


public class LeverScript : MonoBehaviour, Iinteractable
{
    public Animator animator;
    public bool leverIsActive = false;
    public bool CharacterInRange = false;
    public UnityEvent onActivatedLever;
    public UnityEvent onToggleLever;
    public UnityEvent onDeactivatedLever;


    public void ExecuteInteraction()
    {
        ToggleLever();
    }


    public void ToggleLever()
    {
        leverIsActive = !leverIsActive;

        SendEvents();

    }

    public void deactivateLever()
    {
        leverIsActive = false;

        SendEvents();
    }

    public void activateLever()
    {
        leverIsActive = true;

        SendEvents();
    }

    private void SendEvents()
    {
        onToggleLever?.Invoke();
        
        if (leverIsActive)
        {
            onActivatedLever?.Invoke();
            animator.SetTrigger("Activate");
        }
        else
        {
            onDeactivatedLever?.Invoke();
            animator.SetTrigger("Deactive");
        }
    }
}