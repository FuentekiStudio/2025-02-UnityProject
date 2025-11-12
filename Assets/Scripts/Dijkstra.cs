using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DijkstraResult
{
    public List<int> Vertices;
    public List<int> Distancia;
    public List<int?> Previo;

    public DijkstraResult()
    {
        Vertices = new List<int>();
        Distancia = new List<int>();
        Previo = new List<int?>();
    }
}

public class Dijkstra
{
    private Graph grafo;

    public Dijkstra(Graph g)
    {
        grafo = g;
    }

    public DijkstraResult Ejecutar(int origen)
    {
        DijkstraResult resultado = new DijkstraResult();

        // 1) Obtener todos los vértices del grafo
        VertexNode aux = grafo.GetOrigin();
        while (aux != null)
        {
            resultado.Vertices.Add(aux.valorNodo);
            aux = aux.sigNodo;
        }

        int cant = resultado.Vertices.Count;

        // Inicializar listas de distancias y previos
        for (int i = 0; i < cant; i++)
        {
            resultado.Distancia.Add(int.MaxValue);
            resultado.Previo.Add(null);
        }

        // 2) Buscar índice del nodo origen
        int indiceOrigen = resultado.Vertices.IndexOf(origen);
        resultado.Distancia[indiceOrigen] = 0;

        // Lista de visitados (solo ints)
        List<int> visitado = new List<int>();

        // 3) Bucle principal
        while (visitado.Count < cant)
        {
            int actual = -1;
            int mejorDist = int.MaxValue;

            // Seleccionar no visitado con menor distancia
            for (int i = 0; i < cant; i++)
            {
                int vert = resultado.Vertices[i];
                int dist = resultado.Distancia[i];

                if (!visitado.Contains(vert) && dist <= mejorDist)
                {
                    mejorDist = dist;
                    actual = vert;
                }
            }

            if (actual == -1)
            {
                break;
            }

            visitado.Add(actual);

            // Obtener nodo actual del grafo
            VertexNode nodoActual = grafo.VertToNode(actual);
            EdgeNode arista = nodoActual.arista;

            // Relajar aristas
            while (arista != null)
            {
                int destino = arista.nodoDestino.valorNodo;
                int peso = arista.pesoArista;

                int idxActual = resultado.Vertices.IndexOf(actual);
                int idxDestino = resultado.Vertices.IndexOf(destino);

                int nueva = resultado.Distancia[idxActual] + peso;

                if (!visitado.Contains(destino) && nueva < resultado.Distancia[idxDestino])
                {
                    resultado.Distancia[idxDestino] = nueva;
                    resultado.Previo[idxDestino] = actual;
                }

                arista = arista.sigArista;
            }
        }

        return resultado;
    }

    //  Reconstrucción del camino desde origen hasta destino
    public List<int> ObtenerCamino(int origen, int destino, DijkstraResult resultado)
    {
        List<int> camino = new List<int>();

        int indiceDestino = resultado.Vertices.IndexOf(destino);
        if (indiceDestino == -1)
        {
            Debug.LogError("El nodo destino no existe en el grafo.");
            return camino;
        }

        if (resultado.Distancia[indiceDestino] == int.MaxValue)
        {
            Debug.LogWarning("No existe camino desde el origen al destino.");
            return camino;
        }

        int? actual = destino;

        // Reconstruir hacia atrás usando la lista de previos
        while (actual != null)
        {
            camino.Insert(0, actual.Value);
            int indice = resultado.Vertices.IndexOf(actual.Value);
            actual = resultado.Previo[indice];
        }

        // Confirmar que el primer nodo sea el origen
        if (camino[0] != origen)
        {
            Debug.LogWarning("El camino reconstruido no inicia en el nodo origen.");
        }

        return camino;
    }

}

//public class Dijsktrakjsdfvjdç : MonoBehaviour
//{
//    public int[] distance;
//    public string[] nodos;

//    private int MinimumDistance(int[] distance, bool[] shortestPathTreeSet, int verticesCount)
//    {
//        int min = int.MaxValue;
//        int minIndex = 0;

//        for (int v = 0; v < verticesCount; ++v)
//        {
//            // obtengo siempre el nodo con la menor distancia calculada
//            // solo lo verifico en los nodos que no tienen seteado ya un camino (shortestPathTreeSet[v] == false)
//            if (shortestPathTreeSet[v] == false && distance[v] <= min)
//            {
//                min = distance[v];
//                minIndex = v;
//            }
//        }

//        // devuelvo el nodo calculado
//        return minIndex;
//    }

//    public void Dijkstra(Graph graph, int source)
//    {
//        // obtengo la matriz de adyacencia del TDA_Grafo
//        //int[,] graph = grafo.MAdy;

//        // obtengo la cantidad de nodos del TDA_Grafo
//        int vertexCount = graph.GetVertexCount();

//        // obtengo el indice del nodo elegido como origen a partir de su valor
//        VertexNode nodeSource = graph.VertToNode(source);

//        // vector donde se van a guardar los resultados de las distancias entre 
//        // el origen y cada vertice del grafo
//        distance = new int[vertexCount];

//        bool[] shortestPathTreeSet = new bool[vertexCount];

//        int[] nodos1 = new int[vertexCount];
//        int[] nodos2 = new int[vertexCount];

//        for (int i = 0; i < vertexCount; ++i)
//        {
//            // asigno un valor maximo (inalcanzable) como distancia a cada nodo
//            // cualquier camino que se encuentre va a ser menor a ese valor
//            // si no se encuentra un camino, este valor maximo permanece y es el 
//            // indica que no hay ningun camino entre el origen y ese nodo
//            distance[i] = int.MaxValue;

//            // seteo en falso al vector que guarda la booleana cuando se encuentra un camino
//            shortestPathTreeSet[i] = false;

//            nodos1[i] = nodos2[i] = -1;
//        }

//        // la distancia al nodo origen es 0
//        distance[source] = 0;
//        nodos1[source] = nodos2[source] = nodeSource.valorNodo;

//        // recorro todos los nodos (vertices)
//        for (int count = 0; count < vertexCount - 1; ++count)
//        {
//            int minDistance = MinimumDistance(distance, shortestPathTreeSet, vertexCount);
//            shortestPathTreeSet[minDistance] = true;

//            // recorro todos los nodos (vertices)
//            for (int v = 0; v < vertexCount; ++v)
//            {
//                // comparo cada nodo (que aun no se haya calculado) contra el que se encontro que tiene la menor distancia al origen elegido
//                if (!shortestPathTreeSet[v] &&
//                    graph.VertToNode(v) != null &&
//                    distance[minDistance] != int.MaxValue &&
//                    distance[minDistance] + graph.VertToNode(v).valorNodo < distance[v])
//                {
//                    // si encontré una distancia menor a la que tenia, la reasigno la nodo
//                    distance[v] = distance[minDistance] + graph.VertToNode(v).valorNodo;
//                    // guardo los nodos para reconstruir el camino
//                    nodos1[v] = graph.Etiqs[minDistance];
//                    nodos2[v] = graph.Etiqs[v];
//                }
//            }
//        }

//        // construyo camino de nodos
//        nodos = new string[vertexCount];

//        for (int i = 0; i < vertexCount; i++)
//        {
//            if (nodos1[i] != -1)
//            {
//                List<int> l1 = new List<int>();
//                l1.Add(nodos1[i]);
//                l1.Add(nodos2[i]);
//                while (l1[0] != nodeSource.valorNodo)
//                {
//                    for (int j = 0; j < vertexCount; j++)
//                    {
//                        if (j != source && l1[0] == nodos2[j])
//                        {
//                            l1.Insert(0, nodos1[j]);
//                            break;
//                        }
//                    }
//                }
//                for (int j = 0; j < l1.Count; j++)
//                {
//                    if (j == 0)
//                    {
//                        nodos[i] = l1[j].ToString();
//                    }
//                    else
//                    {
//                        nodos[i] += "," + l1[j].ToString();
//                    }
//                }
//            }
//        }
//    }
//}