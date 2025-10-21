using UnityEngine;

[RequireComponent(typeof(LeverScript))]
[DisallowMultipleComponent]
public class LeverToEvent : MonoBehaviour
{
    [Header("Targets")]
    [Tooltip("Referencia al PasswordPuzzle que debe escuchar esta palanca")]
    public PasswordPuzzle targetPuzzle;

    [Tooltip("EventQueue used to serialize inputs in THIS scene.")]
    public EventQueue eventQueue;

    [Header("Config")]
    [Tooltip("Código base de esta palanca (único por palanca: 1, 2, 3...)")]
    public int leverBaseCode = 1; // el número que antes pasabamos por OnActivated

    private LeverScript lever;

    void Awake()
    {
        lever = GetComponent<LeverScript>();
    }

    // Llamamos a este método desde LeverScript.onToggleLever (vía Inspector)
    public void OnActivated()
    {
        if (targetPuzzle == null || eventQueue == null) return;
        eventQueue.AddEvent(new LeverInputEvent(targetPuzzle, leverBaseCode)); // siempre positivos

    }


    //// For convenience: auto-find a queue in parents if we forget to assign it.
    //void OnValidate()
    //{
    //    if (!eventQueue)
    //        eventQueue = GetComponentInParent<EventQueue>();
    //}
}
