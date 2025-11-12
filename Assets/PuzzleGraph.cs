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


    Coroutine delayedVictoryCoroutine;

    public gateScript bill;
    
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
        /*if (victory)
        {
            Victory();
        }*/
    }

    public void AddEdge(PuzzleNode node1, PuzzleNode node2)
    {
        graph.AddEdge(node1.id, node2.id, 1);

        CheckVictory();

        /*
        if (delayedVictoryCoroutine == null)
        {
            Debug.Log("star new rutine");
            delayedVictoryCoroutine = StartCoroutine(DelayedVictory());
        }
        */

    }

    public void DeleteEdge(PuzzleNode node1, PuzzleNode node2)
    {
        graph.DeleteEdge(graph.VertToNode(node1.id), node2.id);
        CheckVictory();
    }

    public bool WinCondition()
    {
        DijkstraResult result = dijkstra.Ejecutar(originNode.id);

        Debug.Log("Distancias:");
        for(int i = 0; i < result.Vertices.Count; i++)
        {
            Debug.Log("Nodo " + result.Vertices[i] + ": " + result.Distancia[i]);
        }
        Debug.Log("___________________");

        List<int> path = dijkstra.ObtenerCamino(originNode.id, destinyNode.id, result);
        int indiceDestino = result.Vertices.IndexOf(destinyNode.id);
        int distance = result.Distancia[indiceDestino];

        string pathString = "Path: ";
        foreach (int value in path)
        {
            pathString += value + " ";
        }
        Debug.Log("___________________");

        if (path.Count != 0)
        {
            Debug.Log("Distancia recibida: " + distance + "m.");
            if (distance <= maxDistance && distance >0)
            {
                Debug.Log("todo bien si forte lleg� todo");
                return true;
            }
            else
            {
                return false;
            }
            
        }

        return false;
    }

    public void CheckVictory()
    {
        
        bool win = WinCondition();
        if (win){
            bill.openedGate = false;
            bill.ChangeSprite();
        }else{
            bill.openedGate = true;
            bill.ChangeSprite();
        }

    }


    IEnumerator DelayedVictory()
    {
        yield return new WaitForSeconds(0);

        DijkstraResult result = dijkstra.Ejecutar(originNode.id);

        Debug.Log("Distancias:");
        foreach (int value in result.Distancia)
        {
            Debug.Log(value);
        }
        Debug.Log("___________________");

        List<int> path = dijkstra.ObtenerCamino(originNode.id, destinyNode.id, result);
        int indiceDestino = result.Vertices.IndexOf(destinyNode.id);
        bool win = WinCondition();
        if (win){
            bill.openedGate = false;
            bill.ChangeSprite();
        }else{
            bill.openedGate = true;
            bill.ChangeSprite();
        }
    }
}
