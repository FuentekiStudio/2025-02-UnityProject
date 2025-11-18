using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleNode : MonoBehaviour
{
    [SerializeField] List<NodeEdge> edges;

    [SerializeField] public int id;

    public PuzzleGraph graph;

    private float rotation = 0f;
    private float speed = 1f;
    private float step = 60f;

    // Start is called before the first frame update
    void Start()
    {
        rotation = gameObject.transform.rotation.eulerAngles.z;
        //Debug.Log("Nodo: " + rotation);
        foreach (NodeEdge edge in edges)
        {
            if (edge.node == null)
            {
                edge.node = this;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, Quaternion.Euler( new Vector3(0f, 0f, rotation)), speed * Time.deltaTime);
    }


    public void Rotate()
    {
        rotation += step;
    }

    public void AristandoAndo(NodeEdge edge)
    {
        graph.AddEdge(this, edge.node);
    }

    public void Desaristando(NodeEdge edge)
    {
        graph.DeleteEdge(this, edge.node);
    }
}
