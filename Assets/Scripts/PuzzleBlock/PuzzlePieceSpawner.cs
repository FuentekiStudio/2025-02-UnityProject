using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzlePieceSpawner : MonoBehaviour
{
    [System.Serializable]
    public class PieceData
    {
        public GameObject piecePrefab; // El prefab específico para cada pieza
        public Vector2 position; // La posición en la cuadrícula donde se debe colocar la pieza
    }

    [SerializeField] private List<PieceData> piecesToSpawn = new List<PieceData>(); // Lista de prefabs y sus posiciones

    [SerializeField] private Vector2Int gridSize = new Vector2Int(6, 6); // Tamaño de la cuadrícula (6x6)

    [SerializeField] private Vector2 keyPiecePosition; // Posición de la pieza clave (puedes asignarla en el Inspector)
    [SerializeField] private GameObject keyPiecePrefab; // Prefab de la pieza clave

    private GameObject[] obstacles;

    void Start()
    {
        // Obtener todos los objetos con el tag "Obstacles" para evitar colisiones
        obstacles = GameObject.FindGameObjectsWithTag("Obstacles");

        // Colocar las piezas movibles en las posiciones especificadas
        SpawnMovablePieces();

        // Colocar la pieza clave en su posición
        SpawnKeyPiece();
    }

    // Método para colocar las piezas movibles
    void SpawnMovablePieces()
    {
        foreach (PieceData pieceData in piecesToSpawn)
        {
            if (!IsPositionBlocked(pieceData.position))
            {
                // Instanciar pieza movible en la posición válida usando el prefab específico
                Instantiate(pieceData.piecePrefab, new Vector3(pieceData.position.x, pieceData.position.y, 0), Quaternion.identity);
            }
            else
            {
                Debug.LogWarning($"La posición {pieceData.position} está bloqueada por un obstáculo. No se puede colocar la pieza.");
            }
        }
    }

    // Método para colocar la pieza clave
    void SpawnKeyPiece()
    {
        if (!IsPositionBlocked(keyPiecePosition))
        {
            // Instanciar la pieza clave en la posición válida
            Instantiate(keyPiecePrefab, new Vector3(keyPiecePosition.x, keyPiecePosition.y, 0), Quaternion.identity);
        }
        else
        {
            Debug.LogWarning($"La posición de la pieza clave {keyPiecePosition} está bloqueada por un obstáculo.");
        }
    }

    // Método para verificar si la posición está bloqueada por un obstáculo
    bool IsPositionBlocked(Vector2 position)
    {
        foreach (var obstacle in obstacles)
        {
            Vector2 obstaclePos = new Vector2(obstacle.transform.position.x, obstacle.transform.position.y);
            if (position == obstaclePos)
            {
                return true; // Posición bloqueada por un obstáculo
            }
        }
        return false; // Posición libre
    }
}
