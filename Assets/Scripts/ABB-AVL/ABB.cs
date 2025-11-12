using System.Collections.Generic;
using UnityEngine;

#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
public class ABB : ABBTDA
{
    public NodeABB? root = null;

    public int RootInfo()
    {
        return root.info;
    }

    public bool IsEmpty()
    {
        return (root == null);
    }

    public void Inicialize()
    {
        root = null;
    }

    public NodeABB RightChild()
    {
        return root.rightChild;
    }

    public NodeABB LeftChild()
    {
        return root.leftChild;
    }

    public void Add(int x)
    {
        NodeABB newItem = new NodeABB(x);
        if (root == null)
        {
            root = newItem;
        }
        else
        {
            root = AddElement(root, newItem);
        }
    }

    public NodeABB AddElement(NodeABB current, NodeABB x)
    {
        if (current == null)
        {
            current = x;
            return current;
        }
        else if (current.info > x.info)
        {
            current.leftChild = AddElement(current.leftChild, x);
        }
        else if (current.info < x.info)
        {
            current.rightChild = AddElement(current.rightChild, x);
        }

        return current;
    }
    public void Remove(int target)
    {
        root = RemoveElement(ref root, target);
    }

    public NodeABB RemoveElement(ref NodeABB current, int x)
    {
        if (current != null)
        {
            if (current.info == x && (current.leftChild == null) && (current.rightChild == null))
            {
                current = null;
            }
            else if (current.info == x && current.leftChild != null)
            {
                current.info = Greatest(current.leftChild);
                RemoveElement(ref current.leftChild, current.info);
            }
            else if (current.info == x && current.leftChild == null)
            {
                current.info = Smallest(current.rightChild);
                RemoveElement(ref current.rightChild, current.info);
            }
            else if (current.info < x)
            {
                RemoveElement(ref current.rightChild, x);
            }
            else
            {
                RemoveElement(ref current.leftChild, x);
            }
        }
        return current;
    }

    public int Greatest(NodeABB node)
    {
        if (node.rightChild == null)
        {
            return node.info;
        }
        else
        {
            return Greatest(node.rightChild);
        }
    }

    public int Smallest(NodeABB node)
    {
        if (node.leftChild == null)
        {
            return node.info;
        }
        else
        {
            return Smallest(node.leftChild);
        }
    }
    public void DisplayTree(TreeOrderTypes orderType)
    {
        if (root == null)
        {
            Debug.Log("Tree is empty");
            return;
        }
        switch (orderType)
        {
            case TreeOrderTypes.InOrder:
                InOrder(root);
                break;
            case TreeOrderTypes.PreOrder:
                PreOrder(root);
                break;
            case TreeOrderTypes.PostOrder:
                PostOrder(root);
                break;
            case TreeOrderTypes.LevelOrder:
                LevelOrder(root);
                break;
        }

        Debug.Log("__________");
    }

    public int Height(NodeABB currentInTree)
    {
        if (currentInTree == null)
        {
            return -1;
        }
        else
        {
            return (1 + Mathf.Max(Height(currentInTree.leftChild), Height(currentInTree.rightChild)));
        }
    }

    private void PreOrderFE(NodeABB currentInTree)
    {
        if (currentInTree != null)
        {
            // accion mientras recorro //
            Debug.Log("Nodo Padre: " + currentInTree.info.ToString());
            Debug.Log("Altura Izquierda: " + Height(currentInTree.rightChild));
            Debug.Log("Altura Derecha: " + Height(currentInTree.leftChild));
            Debug.Log("_________");
            //                         //

            PreOrderFE(currentInTree.leftChild);
            PreOrderFE(currentInTree.rightChild);
        }
    }

    private void PreOrder(NodeABB currentInTree)
    {
        if (currentInTree != null)
        {
            Debug.Log(currentInTree.info.ToString());
            PreOrder(currentInTree.leftChild);
            PreOrder(currentInTree.rightChild);
        }
    }

    private void InOrder(NodeABB a)
    {
        if (a != null)
        {
            InOrder(a.leftChild);
            Debug.Log(a.info.ToString());
            InOrder(a.rightChild);
        }
    }

    private void PostOrder(NodeABB a)
    {
        if (a != null)
        {
            PostOrder(a.leftChild);
            PostOrder(a.rightChild);
            Debug.Log(a.info.ToString());
        }
    }

    private void LevelOrder(NodeABB nodo)
    {
        Queue<NodeABB> q = new Queue<NodeABB>();

        q.Enqueue(nodo);

        while (q.Count > 0)
        {
            nodo = q.Dequeue();

            Debug.Log(nodo.info.ToString());

            if (nodo.leftChild != null) { q.Enqueue(nodo.leftChild); }

            if (nodo.rightChild != null) { q.Enqueue(nodo.rightChild); }
        }
    }

    private void LevelOrderFE(NodeABB nodo)
    {
        Queue<NodeABB> q = new Queue<NodeABB>();

        q.Enqueue(nodo);

        while (q.Count > 0)
        {
            nodo = q.Dequeue();

            Debug.Log("Padre: " + nodo.info.ToString());

            if (nodo.leftChild != null)
            {
                q.Enqueue(nodo.leftChild);
                Debug.Log("Hijo Izq: " + nodo.leftChild.info.ToString());
            }
            else
            {
                Debug.Log("Hijo Izq: null");
            }

            if (nodo.rightChild != null)
            {
                q.Enqueue(nodo.rightChild);
                Debug.Log("Hijo Der: " + nodo.rightChild.info.ToString());
            }
            else
            {
                Debug.Log("Hijo Der: null");
            }
        }
    }

    public NodeABB Search(NodeABB current, int x)
    {
        if (current == null)
        {
            return null;
        }

        if (current.info == x)
        {
            return current;
        }

        if (x < current.info)
        {
            return Search(current.leftChild, x);
        }
        else
        {
            return Search(current.rightChild, x);
        }
    }

    public NodeABB Search(int key)
    {
        NodeABB node = Search(root, key);
        if (node.info == key)
        {
            return node;
        }

        return null;
    }

    public List<int> GetInOrderList()
    {
        List<int> result = new List<int>();
        CollectInOrder(root, result);
        return result;
    }

    private void CollectInOrder(NodeABB node, List<int> list)
    {
        if (node == null) return;
        CollectInOrder(node.leftChild, list);
        list.Add(node.info);
        CollectInOrder(node.rightChild, list);
    }
}
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.