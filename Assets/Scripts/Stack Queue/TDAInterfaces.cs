using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStack<T>
{
    void Push(T element);

    void Pop();

    T Top();

    bool IsEmpty();

    bool IsFull();
}

public interface IQueue<T>
{
    void Enqueue(T element);

    void Dequeue();

    T First();

    bool IsEmpty();

    bool IsFull();
}

public class TDAStack<T> : IStack<T>
{
    T[] array;
    int arrSize;
    int index;

    public TDAStack(int arrSize)
    {
        array = new T[arrSize];
        this.arrSize = arrSize;
        index = 0;
    }

    public void Push(T element)
    {
        array[index] = element;
        index++;
    }

    public void Pop()
    {
        index--;
    }

    public T Top()
    {
        return array[index];
    }

    public bool IsEmpty()
    {
        return index == 0;
    }

    public bool IsFull()
    {
        return index >= arrSize;
    }

    public override string ToString()
    {
        string stack = "-> ";
        for (int i = 0; i < index; i++)
        {
            stack += array[i].ToString() + ", ";
        }

        return stack;
    }
}
internal class TDAQueue<T> : IQueue<T>
{
    private T[] array;
    private int arrSize;
    int index;

    public int Size => arrSize;
    public int FilledAmount => index;

    public TDAQueue(int arrSize)
    {
        array = new T[arrSize];
        this.arrSize = arrSize;
        index = 0;
    }


    public void Enqueue(T element)
    {
        for (int i = index; i > 0; i--)
        {
            array[i] = array[i - 1];
        }

        array[0] = element;
        index++;
    }

    public void Dequeue()
    {
        index--;
    }

    public T First()
    {
        return array[index - 1];
    }

    public bool IsEmpty()
    {
        return index == 0;
    }

    public bool IsFull()
    {
        return (index == arrSize);
    }

    public override string ToString()
    {
        string queue = "-> ";

        for (int i = index - 1; i >= 0; i--)
        {
            queue += array[i].ToString() + ", ";
        }

        return queue;
    }
}
