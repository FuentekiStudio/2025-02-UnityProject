public interface ABBTDA
{
    int RootInfo();
    NodeABB LeftChild();
    NodeABB RightChild();
    bool IsEmpty();
    void Inicialize();
    void Add(int x);
    NodeABB AddElement(NodeABB current, NodeABB x);
    void Remove(int x);
    NodeABB RemoveElement(ref NodeABB current, int x);
    void Search(int key);
    NodeABB Search(NodeABB current, int target);
}