using UnityEngine;

public class LeverToEvent : MonoBehaviour
{
    [Tooltip("Referencia al PasswordPuzzle que debe escuchar esta palanca")]
    public PasswordPuzzle targetPuzzle;

    [Tooltip("Código base de esta palanca (único por palanca: 1, 2, 3...)")]
    public int leverBaseCode = 1; // el número que antes pasabamos por OnActivated

    private LeverScript lever;

    void Awake()
    {
        lever = GetComponent<LeverScript>();
        if (lever == null)
            Debug.LogError("LeverToEvent requiere LeverScript en el mismo GameObject.");
    }

    // Llamamos a este método desde LeverScript.onToggleLever (vía Inspector)
    public void OnActivated()
    {
        if (targetPuzzle == null || EventQueue.Instance == null) return;
        EventQueue.Instance.AddEvent(new LeverInputEvent(targetPuzzle, leverBaseCode)); // siempre positivos

        // Esto si queremos encolar numeros negativos tambien
        //if (targetPuzzle == null || EventQueue.Instance == null) return;

        //// Estado actual => código (+base para activo / -base para inactivo)
        //int code = lever.leverIsActive ? leverBaseCode : -leverBaseCode;

        //EventQueue.Instance.AddEvent(new LeverInputEvent(targetPuzzle, code));
    }
}
