using UnityEngine;

public class CenterCameraOnGrid : MonoBehaviour
{
    [SerializeField] private Vector2Int gridSize = new Vector2Int(6, 6); // Tamaño de la cuadrícula (6x6)
    [SerializeField] private float cameraZPosition = -10f; // Posición Z de la cámara (normalmente -10 en 2D)

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        CenterCamera();
    }

    // Método para centrar la cámara en el centro de la cuadrícula
    private void CenterCamera()
    {
        // Calcula el centro de la cuadrícula
        Vector2 gridCenter = new Vector2(gridSize.x / 2f, gridSize.y / 2f);

        // Establecer la posición de la cámara en el centro de la cuadrícula
        mainCamera.transform.position = new Vector3(gridCenter.x, gridCenter.y, cameraZPosition);
    }
}
