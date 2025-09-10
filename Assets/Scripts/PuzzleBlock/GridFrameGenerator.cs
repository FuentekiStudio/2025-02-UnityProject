using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridFrameGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject squarePrefab; // El prefab del cuadrado de 1x1

    [SerializeField]
    private Vector2Int gridSize = new Vector2Int(6, 6); // Tamaño de la cuadrícula (6x6)

    [SerializeField]
    private Vector2 exitPosition; // La posición de la abertura en el borde

    void Start()
    {
        // Asignamos la posición de salida en el centro del borde inferior de la cuadrícula
        exitPosition = new Vector2(gridSize.x / 2, 0); // Centra la salida en la parte inferior

        // Verificar si el prefab está asignado
        if (squarePrefab != null)
        {
            GenerateGridFrame();
        }
        else
        {
            Debug.LogError("¡El prefab del cuadrado no está asignado!");
        }
    }

    void GenerateGridFrame()
    {
        // Generar los bordes superiores e inferiores de la cuadrícula
        for (int x = 0; x < gridSize.x; x++)
        {
            // Borde superior
            CreateSquareAtPosition(new Vector2(x, gridSize.y - 1));
            // Borde inferior
            CreateSquareAtPosition(new Vector2(x, 0));
        }

        // Generar los bordes laterales de la cuadrícula
        for (int y = 1; y < gridSize.y - 1; y++) // Evita las esquinas ya generadas
        {
            // Borde izquierdo
            CreateSquareAtPosition(new Vector2(0, y));
            // Borde derecho
            CreateSquareAtPosition(new Vector2(gridSize.x - 1, y));
        }
    }

    // Método para crear un cuadrado en una posición determinada
    void CreateSquareAtPosition(Vector2 position)
    {
        // Si la posición corresponde a la salida, no creamos el cuadrado
        if (position == exitPosition)
        {
            return;
        }

        // Instancia el prefab del cuadrado en la posición dada
        GameObject newSquare = Instantiate(squarePrefab, position, Quaternion.identity);
        newSquare.transform.parent = this.transform; // Asegurarse de que el objeto generado esté bajo el padre adecuado

        // Asigna el tag "Obstacles" a los objetos generados para actuar como bordes
        newSquare.tag = "Obstacles";
    }
}
