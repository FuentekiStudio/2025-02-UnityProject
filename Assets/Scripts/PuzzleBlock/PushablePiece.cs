using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushablePiece : MonoBehaviour
{
    [SerializeField] private bool isHorizontal; // Define si la pieza es horizontal o vertical

    private Vector3 startPosition; // La posición inicial de la pieza antes de moverla
    private bool isDragging = false; // Si el jugador está arrastrando la pieza
    private Vector2 dragDirection; // Dirección en la que la pieza se puede mover (horizontal o vertical)

    private Rigidbody2D rb2d;
    private BoxCollider2D boxCollider;

    private Vector3 lastValidPosition; // Última posición válida en la grilla

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();

        // Definir la dirección de movimiento permitida según si es horizontal o vertical
        if (isHorizontal)
        {
            dragDirection = Vector2.right; // Puede moverse en el eje X (izquierda/derecha)
        }
        else
        {
            dragDirection = Vector2.up; // Puede moverse en el eje Y (arriba/abajo)
        }

        // Guardar la posición inicial como la última posición válida
        lastValidPosition = transform.position;

        // Configurar el Rigidbody2D como cinemático para controlar el movimiento manualmente
        rb2d.isKinematic = true;
    }

    void FixedUpdate()
    {
        if (isDragging)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 targetPosition;

            // Para piezas horizontales, mover solo en el eje X en incrementos de 1
            if (isHorizontal)
            {
                float deltaX = Mathf.Round(mousePosition.x) - startPosition.x;
                if (Mathf.Abs(deltaX) >= 1f) // Solo mover si el cambio en X es de al menos 1 unidad
                {
                    targetPosition = new Vector3(startPosition.x + Mathf.Sign(deltaX), startPosition.y, startPosition.z);
                }
                else
                {
                    return; // No mover si el cambio es menor que 1
                }
            }
            else
            {
                // Para piezas verticales, mover solo en el eje Y en incrementos de 1
                float deltaY = Mathf.Round(mousePosition.y) - startPosition.y;
                if (Mathf.Abs(deltaY) >= 1f) // Solo mover si el cambio en Y es de al menos 1 unidad
                {
                    targetPosition = new Vector3(startPosition.x, startPosition.y + Mathf.Sign(deltaY), startPosition.z);
                }
                else
                {
                    return; // No mover si el cambio es menor que 1
                }
            }

            // Verificar si podemos mover la pieza (sin colisiones)
            if (!IsBlocked(targetPosition))
            {
                rb2d.MovePosition(targetPosition); // Mover la pieza usando Rigidbody.MovePosition
                startPosition = targetPosition; // Actualizar la nueva posición inicial para el siguiente movimiento
            }
        }
    }

    // Se llama cuando se hace clic en la pieza
    void OnMouseDown()
    {
        isDragging = true; // Iniciar el arrastre
        startPosition = transform.position; // Guardar la posición inicial
    }

    // Se llama cuando se suelta el botón del mouse
    void OnMouseUp()
    {
        isDragging = false; // Terminar el arrastre
    }

    // Verificar colisiones antes de mover la pieza
    private bool IsBlocked(Vector3 targetPosition)
    {
        Vector2 direction = (targetPosition - transform.position).normalized;
        float distance = Vector2.Distance(transform.position, targetPosition);

        // Realizar un BoxCast desde la posición actual hacia la dirección deseada
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, direction, distance);

        // Verificar si la colisión es con otro objeto (obstáculo o pieza movible)
        if (hit.collider != null && hit.collider.gameObject != gameObject)
        {
            if (hit.collider.CompareTag("Obstacles") || hit.collider.CompareTag("ObjectsToPush"))
            {
                Debug.Log("Colisión detectada con: " + hit.collider.gameObject.name);
                return true; // La pieza está bloqueada por un obstáculo o una pieza movible
            }
        }

        return false; // No está bloqueada, puede moverse
    }

    // Detectar colisiones al tocar otros objetos
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacles") || collision.gameObject.CompareTag("ObjectsToPush"))
        {
            Debug.Log("Colisión directa detectada con: " + collision.gameObject.name);
            isDragging = false; // Detener el movimiento en caso de colisión directa
        }
    }

   
}
