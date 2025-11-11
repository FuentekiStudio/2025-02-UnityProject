using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeABB
{
    public int info;
    public NodeABB? leftChild = null;
    public NodeABB? rightChild = null;

    public NodeABB(int info)
    {
        this.info = info;
    }
}
