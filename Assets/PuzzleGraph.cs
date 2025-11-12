using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleGraph : MonoBehaviour
{
    [SerializeField] List<PuzzleNode> nodes;

    private Graph graph;

    public PuzzleNode originNode;
    public PuzzleNode destinyNode;
    public int maxDistance;
    
    private Dijkstra dijkstra;

    public gateScript bill;
    private bool victory = false;

    // Start is called before the first frame update
    void Start()
    {
        graph = new Graph();
        graph.InitGraph();
        dijkstra = new Dijkstra(graph);

        foreach (PuzzleNode node in nodes) 
        {
            graph.AddVertex(node.id);
            node.graph = this;
        }

        //originNode = nodes[0];
        //destinyNode = nodes[nodes.Count -1];

    }

    private void Update()
    {
        if (victory)
        {
            Victory();
        }
    }

    public void AddEdge(PuzzleNode node1, PuzzleNode node2)
    {
        graph.AddEdge(node1.id, node2.id, 1);
        
        DijkstraResult result = dijkstra.Ejecutar(originNode.id);

        Debug.Log("Distancias:");
        foreach (int value in result.Distancia)
        {
            Debug.Log(value);
        }
        Debug.Log("___________________");
        List<int> path = dijkstra.ObtenerCamino(originNode.id, destinyNode.id, result);
        int indiceDestino = result.Vertices.IndexOf(destinyNode.id);
        WinCondition(path, result.Distancia[indiceDestino]);
    }

    public void DeleteEdge(PuzzleNode node1, PuzzleNode node2)
    {
        graph.DeleteEdge(graph.VertToNode(node1.id), node2.id);
    }

    public void WinCondition(List<int> path, int distance)
    {
        Debug.Log("Path:");
        foreach (int value in path)
        {
            Debug.Log(value);
        }
        Debug.Log("___________________");

        if (path.Count != 0)
        {
            if (distance <= maxDistance)
            {
                Debug.Log("todo bien si forte llegó todo");
                victory = true;
            }
            else
            {
                victory = false;
            }
            
            Debug.Log("Distancia recibida: " + distance + "m.");
        }
    }

    public void Victory()
    {
        bill.ChangeSprite();
        victory = false;
    }
}
