using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

[ExecuteAlways] // Ejecuta el script siempre, tanto en el Editor como en tiempo de ejecuci�n
public class ItemScaler : MonoBehaviour
{
    [Header("Target Size for Items (in World Units)")]
    public Vector2 targetSize = new Vector2(1f, 1f); // Tama�o deseado para todos los �tems

    private void Awake()
    {
        AdjustAllItems();
    }

    private void Start()
    {
        AdjustAllItems();
    }

    private void OnValidate()
    {
        AdjustAllItems();
    }

    private void AdjustAllItems()
    {
        if (transform.childCount == 0) return;

        foreach (Transform child in transform)
        {
            AdjustItemSize(child);
        }
    }

    private void AdjustItemSize(Transform itemTransform)
    {
        // Verificar si el hijo tiene un SpriteRenderer y un BoxCollider2D
        SpriteRenderer sr = itemTransform.GetComponent<SpriteRenderer>();
        BoxCollider2D collider = itemTransform.GetComponent<BoxCollider2D>();
        CircleCollider2D colliderCircle = itemTransform.GetComponent<CircleCollider2D>();

        if (sr != null)
        {
            // Obtener el tama�o original del sprite
            float spriteWidth = sr.sprite.bounds.size.x;
            float spriteHeight = sr.sprite.bounds.size.y;

            // Calcular el factor de escala para que coincida con el tama�o deseado
            float scaleX = targetSize.x / spriteWidth;
            float scaleY = targetSize.y / spriteHeight;

            // Ajustar la escala del objeto
            itemTransform.localScale = new Vector3(scaleX, scaleY, 1f);

            // Si existe un BoxCollider2D, ajustar su tama�o despu�s de cambiar la escala
            if (collider != null)
            {
                collider.size = new Vector2(sr.sprite.bounds.size.x, sr.sprite.bounds.size.y);
                collider.offset = Vector2.zero;
            }

//            Debug.Log($"[ItemScaler] Item: {itemTransform.name}, Scale: {itemTransform.localScale}, Collider Size: {collider.size}");
        }
    }
}
