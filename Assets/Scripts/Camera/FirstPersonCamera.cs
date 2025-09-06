using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    public float mouseSensitivity = 100f; // sensibilidad del mouse
    public Transform playerBody;          // referencia al cuerpo del jugador (cápsula)

    private float xRotation = 0f;

    void Start()
    {
        // Oculta y bloquea el cursor en el centro de la pantalla
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Ejes del mouse
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Rotación en vertical (limitada para no girar la cabeza 360°)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Aplica la rotación vertical solo a la cámara
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Aplica la rotación horizontal al cuerpo del jugador
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
