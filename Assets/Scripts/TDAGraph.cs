using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public interface TDAGraph
    {
        void InitGraph();
        void AddVertex(int v);
        void DeleteVertex(int v);
        SetTDA VertexSet();
        void AddEdge(int v1, int v2, int peso);
        void DeleteEdge(int v1, int v2);
        bool ExistsEdge(int v1, int v2);
        int WeightEdge(int v1, int v2);
    }

    public class VertexNode
    {
        public int valorNodo;
        public EdgeNode arista;
        public VertexNode sigNodo;
    }

    public class EdgeNode
    {
        public int pesoArista;
        public VertexNode nodoDestino;
        public EdgeNode sigArista;
    }

    public class Graph : TDAGraph
    {
        VertexNode origin;

        public void InitGraph()
        {
            origin = null;
        }

        public void AddVertex(int v)
        {
            //El vertice se inserta al inicio de la lista de nodos
            VertexNode aux = new VertexNode();
            aux.valorNodo = v;
            aux.arista = null;
            aux.sigNodo = origin;
            origin = aux;
        }

        /*
        * Para agregar una nueva arista al grafo , primero se deben
        * buscar los nodos entre los cuales se va agregar la arista ,
        * y luego se inserta sobre la lista de adyacentes del nodo
        * origen (en este caso nombrado como v1)
        */

        public void AddEdge(int vertex1, int vertex2, int peso)
        {
            VertexNode node1 = VertToNode(vertex1);
            VertexNode node2 = VertToNode(vertex2);
            //La nueva arista se inserta al inicio de la lista
            //de nodos adyacentes del nodo origen
            EdgeNode aux = new EdgeNode();
            aux.pesoArista = peso;
            aux.nodoDestino = node2;
            aux.sigArista = node1.arista;
            node1.arista = aux;
        }

        public VertexNode VertToNode(int value)
        {
            VertexNode aux = origin;
            while (aux != null && aux.valorNodo != value)
            {
                aux = aux.sigNodo;
            }
            return aux;
        }

        public void DeleteVertex(int value)
        {
            //Se recorre la lista de v´ertices para remover el nodo v
            //y las aristas con este v´ertice.
            // Distingue el caso que sea el primer nodo
            if (origin.valorNodo == value)
            {
                origin = origin.sigNodo;
            }
            VertexNode aux = origin;
            while (aux != null)
            {
                // remueve de aux todas las aristas hacia v
                this.DeleteEdge(aux, value);
                if (aux.sigNodo != null && aux.sigNodo.valorNodo == value)
                {
                    //Si el siguiente nodo de aux es v, lo elimina
                    aux.sigNodo = aux.sigNodo.sigNodo;
                }
                aux = aux.sigNodo;
            }
        }

        /*
        * Si en las aristas del nodo existe
        * una arista hacia v, la elimina
        */
        public void DeleteEdge(VertexNode nodo, int vertex)
        {
            EdgeNode aux = nodo.arista;
            if (aux != null)
            {
                //Si la arista a eliminar es la primera en
                //la lista de nodos adyacentes
                if (aux.nodoDestino.valorNodo == vertex)
                {
                    nodo.arista = aux.sigArista;
                }
                else
                {
                    while (aux.sigArista != null && aux.sigArista.nodoDestino.valorNodo != vertex)
                    {
                        aux = aux.sigArista;
                    }
                    if (aux.sigArista != null)
                    {
                        // Quita la referencia a la arista hacia v
                        aux.sigArista = aux.sigArista.sigArista;
                    }
                }
            }
        }

    

    public void DeleteEdge(int v1, int v2)
    {
        throw new System.NotImplementedException();
    }

    public bool ExistsEdge(int v1, int v2)
    {
        throw new System.NotImplementedException();
    }

    public int WeightEdge(int v1, int v2)
    {
        throw new System.NotImplementedException();
    }



    public SetTDA VertexSet()
    {
        throw new System.NotImplementedException();
    }

    


    }

