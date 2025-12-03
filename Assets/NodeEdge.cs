using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeEdge : MonoBehaviour
{
    public bool _aristando = false;
    public PuzzleNode node;

    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        NodeEdge edge = collision.GetComponent<NodeEdge>();

        if (edge != null)
        {
            _aristando=true;
            node.AristandoAndo(edge);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        NodeEdge edge = collision.GetComponent<NodeEdge>();

        if (edge != null)
        {

            node.Desaristando(edge);
        }
    }
}
