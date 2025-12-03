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
        //float rotationVel;
        //rotationVel = Mathf.Lerp(gameObject.transform.rotation.z, rotation, speed * Time.deltaTime);
        //Debug.Log(rotationVel);

        gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, Quaternion.Euler( new Vector3(0f, 0f, rotation)), speed * Time.deltaTime);

        //gameObject.transform.rotation = Quaternion.Euler(gameObject.transform.rotation.eulerAngles + new Vector3(0f, 0f, rotationVel));
    }


    public void Rotate()
    {
        rotation += step;
        //gameObject.transform.rotation = Quaternion.Euler(gameObject.transform.rotation.eulerAngles + new Vector3(0f, 0f, step));
    }

    public void AristandoAndo(NodeEdge edge)
    {
        //Hace algo
        //Debug.Log("Aristando");
        graph.AddEdge(this, edge.node);
    }

    public void Desaristando(NodeEdge edge)
    {
        //Algo hace
        graph.DeleteEdge(this, edge.node);
    }
}
