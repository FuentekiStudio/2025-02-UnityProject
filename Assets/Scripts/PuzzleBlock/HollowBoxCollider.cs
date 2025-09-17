using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HollowBoxCollider : MonoBehaviour
{
    public float width = 5f;
    public float height = 5f;
    public float thickness = 0.1f;

    void Start()
    {
        CreateHollowBox();
    }

    void CreateHollowBox()
    {
        // Create borders
        CreateBorder(new Vector2(0, height / 2), new Vector2(width, thickness));  // Top
        CreateBorder(new Vector2(0, -height / 2), new Vector2(width, thickness)); // Bottom
        CreateBorder(new Vector2(-width / 2, 0), new Vector2(thickness, height)); // Left
        CreateBorder(new Vector2(width / 2, 0), new Vector2(thickness, height));  // Right
    }

    void CreateBorder(Vector2 position, Vector2 size)
    {
        GameObject border = new GameObject("Border");
        border.transform.parent = transform;
        border.transform.localPosition = position;
        BoxCollider2D collider = border.AddComponent<BoxCollider2D>();
        collider.size = size;
    }
}
