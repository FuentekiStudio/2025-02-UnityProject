using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
public class AVL : ABBTDA
{
    NodeABB? root;

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

    public void Add(int data)
    {
        NodeABB newItem = new NodeABB(data);
        if (root == null)
        {
            root = newItem;
        }
        else
        {
            root = AddElement(root, newItem);
        }
    }

    public NodeABB AddElement(NodeABB current, NodeABB n)
    {
        if (current == null)
        {
            current = n;
            return current;
        }
        else if (n.info < current.info)
        {
            current.leftChild = AddElement(current.leftChild, n);
            current = BalanceTree(current);
        }
        else if (n.info > current.info)
        {
            current.rightChild = AddElement(current.rightChild, n);
            current = BalanceTree(current);
        }
        return current;
    }
    private NodeABB BalanceTree(NodeABB current)
    {
        int b_factor = BalanceFactor(current);
        if (b_factor > 1)
        {
            if (BalanceFactor(current.leftChild) > 0)
            {
                current = RotateLL(current);
            }
            else
            {
                current = RotateLR(current);
            }
        }
        else if (b_factor < -1)
        {
            if (BalanceFactor(current.rightChild) > 0)
            {
                current = RotateRL(current);
            }
            else
            {
                current = RotateRR(current);
            }
        }
        return current;
    }

    public void Remove(int target)
    {
        root = RemoveElement(ref root, target);
    }

    public NodeABB RemoveElement(ref NodeABB current, int target)
    {
        NodeABB parent;
        if (current == null)
        { return null; }
        else
        {
            //left subtree
            if (target < current.info)
            {
                current.leftChild = RemoveElement(ref current.leftChild, target);
                if (BalanceFactor(current) == -2)//here
                {
                    if (BalanceFactor(current.rightChild) <= 0)
                    {
                        current = RotateRR(current);
                    }
                    else
                    {
                        current = RotateRL(current);
                    }
                }
            }
            //right subtree
            else if (target > current.info)
            {
                current.rightChild = RemoveElement(ref current.rightChild, target);
                if (BalanceFactor(current) == 2)
                {
                    if (BalanceFactor(current.leftChild) >= 0)
                    {
                        current = RotateLL(current);
                    }
                    else
                    {
                        current = RotateLR(current);
                    }
                }
            }
            //if target is found
            else
            {
                if (current.rightChild != null)
                {
                    //delete its inorder successor
                    parent = current.rightChild;
                    while (parent.leftChild != null)
                    {
                        parent = parent.leftChild;
                    }
                    current.info = parent.info;
                    current.rightChild = RemoveElement(ref current.rightChild, parent.info);
                    if (BalanceFactor(current) == 2)//rebalancing
                    {
                        if (BalanceFactor(current.leftChild) >= 0)
                        {
                            current = RotateLL(current);
                        }
                        else { current = RotateLR(current); }
                    }
                }
                else
                {   //if current.left != null
                    return current.leftChild;
                }
            }
        }
        return current;
    }

    public void Search(int key)
    {
        if (Search(root, key).info == key)
        {
            Debug.Log(key + " was found!");
        }
        else
        {
            Debug.Log("Nothing found!");
        }
    }

    public NodeABB Search(NodeABB current, int target)
    {

        if (target < current.info)
        {
            if (target == current.info)
            {
                return current;
            }
            else
                return Search(current.leftChild, target);
        }
        else
        {
            if (target == current.info)
            {
                return current;
            }
            else
                return Search(current.rightChild, target);
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

        Debug.Log("___________");
    }

    private void InOrder(NodeABB current)
    {
        if (current != null)
        {
            InOrder(current.leftChild);
            Debug.Log("(" + current.info + ") ");
            InOrder(current.rightChild);
        }
    }

    private void PreOrder(NodeABB current)
    {
        if (current != null)
        {
            Debug.Log("(" + current.info + ") ");
            PreOrder(current.leftChild);
            PreOrder(current.rightChild);
        }
    }

    private void PostOrder(NodeABB current)
    {
        if (current != null)
        {
            PostOrder(current.leftChild);
            PostOrder(current.rightChild);
            Debug.Log("(" + current.info + ") ");
        }
    }

    private void LevelOrder(NodeABB node)
    {
        Queue<NodeABB> q = new Queue<NodeABB>();

        q.Enqueue(node);

        while (q.Count > 0)
        {
            node = q.Dequeue();

            Debug.Log("(" + node.info + ") ");

            if (node.leftChild != null) { q.Enqueue(node.rightChild); }

            if (node.leftChild != null) { q.Enqueue(node.rightChild); }
        }
    }
    private int Max(int l, int r)
    {
        return l > r ? l : r;
    }
    private int GetHeight(NodeABB current)
    {
        int height = 0;
        if (current != null)
        {
            int l = GetHeight(current.leftChild);
            int r = GetHeight(current.rightChild);
            int m = Max(l, r);
            height = m + 1;
        }
        return height;
    }
    private int BalanceFactor(NodeABB current)
    {
        int l = GetHeight(current.leftChild);
        int r = GetHeight(current.rightChild);
        int b_factor = l - r;
        return b_factor;
    }
    private NodeABB RotateRR(NodeABB parent)
    {
        NodeABB pivot = parent.rightChild;
        parent.rightChild = pivot.leftChild;
        pivot.leftChild = parent;
        return pivot;
    }
    private NodeABB RotateLL(NodeABB parent)
    {
        NodeABB pivot = parent.leftChild;
        parent.leftChild = pivot.rightChild;
        pivot.rightChild = parent;
        return pivot;
    }
    private NodeABB RotateLR(NodeABB parent)
    {
        NodeABB pivot = parent.leftChild;
        parent.leftChild = RotateRR(pivot);
        return RotateLL(parent);
    }
    private NodeABB RotateRL(NodeABB parent)
    {
        NodeABB pivot = parent.rightChild;
        parent.rightChild = RotateLL(pivot);
        return RotateRR(parent);
    }
}
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.