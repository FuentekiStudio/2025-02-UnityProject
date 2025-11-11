#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
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
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.