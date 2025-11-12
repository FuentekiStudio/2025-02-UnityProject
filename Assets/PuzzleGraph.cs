using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleGraph : MonoBehaviour
{
    [SerializeField] List<PuzzleNode> nodes;

    private Graph graph;

    public PuzzleNode originNode;
    public PuzzleNode destinyNode;
    
    
    // Start is called before the first frame update
    void Start()
    {
        graph = new Graph();
        graph.InitGraph();

        foreach (PuzzleNode node in nodes) 
        {
            graph.AddVertex(node.id);
            node.graph = this;
        }

        originNode = nodes[0];
        destinyNode = nodes[nodes.Count -1];

    }

    public void AddEdge(PuzzleNode node1, PuzzleNode node2)
    {
        graph.AddEdge(node1.id, node2.id, 1);
    }

    public void DeleteEdge(PuzzleNode node1, PuzzleNode node2)
    {
        graph.DeleteEdge(graph.VertToNode(node1.id), node2.id);
    }

    
}
